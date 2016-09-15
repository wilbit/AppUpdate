using System;
using System.IO;
using Wilbit.AppUpdate.Configuration;
using Wilbit.AppUpdate.Exceptions;
using Wilbit.AppUpdate.Helpers;

namespace Wilbit.AppUpdate
{
    public sealed class AppUpdater : IDisposable
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        private readonly IFeedSource _feedSource;
        private readonly ILogger _logger;
        private readonly FileDownloader _downloader = new FileDownloader();
        private readonly UpdateScriptGenerator _scriptGenerator = new UpdateScriptGenerator();
        private readonly UpdateServer _updateServer = new UpdateServer();
        private volatile UpdateLoopTimerHolder _updateLoopTimerHolder;

        private readonly CountErrorLogger _checkUpdatesErrorLoger;
        private readonly CountErrorLogger _updateErrorLoger;

        public AppUpdater(IFeedSource feedSource, ILogger logger)
        {
            if (feedSource == null) throw new ArgumentNullException(nameof(feedSource));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _feedSource = feedSource;
            _logger = logger;

            _checkUpdatesErrorLoger = new CountErrorLogger(_logger);
            _updateErrorLoger = new CountErrorLogger(_logger);
        }

        public bool CheckUpdates(IAppInfo appInfo, out ServerVersionInfo serverVersionInfo, bool updateIfVersionsNotEqual = false)
        {
            return CheckUpdates(appInfo, out serverVersionInfo, DefaultTimeout, updateIfVersionsNotEqual);
        }

        public bool CheckUpdates(IAppInfo appInfo, out ServerVersionInfo serverVersionInfo, TimeSpan timeout, bool updateIfVersionsNotEqual = false)
        {
            if (appInfo == null) throw new ArgumentNullException(nameof(appInfo));

            serverVersionInfo = null;
            try
            {
                var feed = _feedSource.GetNextFeed();
                serverVersionInfo = _updateServer.GetServerVersionInfo(feed, timeout);
                var appVersion = appInfo.Version;

                bool result;

                if (updateIfVersionsNotEqual)
                {
                    result = appVersion.CompareTo(serverVersionInfo.Version) != 0;
                }
                else
                {
                    result = appVersion.CompareTo(serverVersionInfo.Version) == -1;
                }

                _checkUpdatesErrorLoger.Success();

                return result;
            }
            catch (Exception e)
            {
                _checkUpdatesErrorLoger.Error(e);
                return false;
            }
        }

        public InstallResultEnum InstallUpdates(IAppInfo appInfo, ServerVersionInfo serverVersionInfo, Action<ProgressInfo> progressDelegate)
        {
            if (appInfo == null) throw new ArgumentNullException(nameof(appInfo));
            if (serverVersionInfo == null) throw new ArgumentNullException(nameof(serverVersionInfo));

            try
            {
                PrepareUpdates(appInfo, serverVersionInfo, progressDelegate);

                var script = _scriptGenerator.GenerateScript(
                    applicationDirectory: appInfo.AppLocation,
                    applicationFileName: appInfo.AppFileName,
                    applicationParams: appInfo.AppParams,
                    updateDirectory: GetUpdateDirectory(appInfo),
                    updateFileName: serverVersionInfo.FileName);

                ShellHelper.ExecuteScript(script);

                _updateErrorLoger.Success();

                return InstallResultEnum.UpdateProcessIsRunning_RequiresRestartOfApplication;
            }
            catch
            {
                _updateErrorLoger.Success();

                return InstallResultEnum.UpdatesNotFound;
            }
        }

        public void PrepareUpdates(IAppInfo appInfo, ServerVersionInfo serverVersionInfo, Action<ProgressInfo> progressDelegate)
        {
            const string requestParams = "source=AppUpdater";
            var sourceFile = UrlHelper.CreateUrl(serverVersionInfo.Feed.Uri, serverVersionInfo.FileName, requestParams);

            var updateDirectory = GetUpdateDirectory(appInfo);
            var destinationFileLocation = Path.Combine(updateDirectory, serverVersionInfo.FileName);

            if (!File.Exists(destinationFileLocation) || !serverVersionInfo.Hash.CheckHashForFile(destinationFileLocation))
            {
                CheckIfInstallerRemoved(destinationFileLocation);

                _downloader.DownloadFile(sourceFile, destinationFileLocation, progressDelegate);
            }
            try
            {
                serverVersionInfo.Hash.CheckHashForFileWithThrowing(destinationFileLocation);
            }
            catch (HashInfoException)
            {
                File.Delete(destinationFileLocation);
                throw;
            }
        }

        private void CheckIfInstallerRemoved(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            catch (Exception e)
            {
                _logger.Error("App update installer wasn't removed", e);
            }
        }

        public void InitializeCheckUpdateLoop(IAppInfo appInfo, TimeSpan period, Action<ServerVersionInfo> newVersionFoundCallback)
        {
            if (_updateLoopTimerHolder != null)
            {
                throw new InvalidOperationException("Check update loop has already initialized");
            }
            _updateLoopTimerHolder = new UpdateLoopTimerHolder(this, appInfo, period, newVersionFoundCallback);
        }

        public void StartCheckingLoop()
        {
            if (_updateLoopTimerHolder == null)
            {
                throw new InvalidOperationException("Check update loop hasn't initialized");
            }
            _updateLoopTimerHolder.Start();
        }

        public void StopCheckingLoop()
        {
            if (_updateLoopTimerHolder == null)
            {
                throw new InvalidOperationException("Check update loop hasn't initialized");
            }
            _updateLoopTimerHolder.Stop();
        }

        private static string GetUpdateDirectory(IAppInfo appInfo)
        {
            var destinationDirectory = Path.Combine(appInfo.AppLocation, "update");

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            return destinationDirectory;
        }

        public void Dispose()
        {
            if (_updateLoopTimerHolder != null)
            {
                _updateLoopTimerHolder.Dispose();
                _updateLoopTimerHolder = null;
            }
        }

        private class CountErrorLogger
        {
            private readonly object _sync = new object();

            private int _count;
            private readonly ILogger _logger;

            public CountErrorLogger(ILogger logger)
            {
                _logger = logger;
            }

            public void Error(Exception ex)
            {
                lock (_sync)
                {
                    _count++;

                    if (_count >= 3)
                    {
                        _logger.Error(ex);
                        _count = 0;
                    }
                }
            }

            public void Success()
            {
                lock (_sync)
                {
                    _count = 0;
                }
            }
        }
    }
}