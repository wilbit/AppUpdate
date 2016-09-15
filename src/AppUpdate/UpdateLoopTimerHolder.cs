using System;
using System.Threading;
using Wilbit.AppUpdate.Configuration;

namespace Wilbit.AppUpdate
{
    internal sealed class UpdateLoopTimerHolder : IDisposable
    {
        private readonly TimeSpan _period;
        private Timer _updateLoopTimer;
        private readonly object _updateLoopTimerLock = new object();
        private volatile bool _stoped;
        private readonly object _loopSync = new object();

        public UpdateLoopTimerHolder(AppUpdater appUpdater, IAppInfo appInfo, TimeSpan period, Action<ServerVersionInfo> newVersionFoundCallback)
        {
            if (appInfo == null) throw new ArgumentNullException(nameof(appInfo));
            if (newVersionFoundCallback == null) throw new ArgumentNullException(nameof(newVersionFoundCallback));

            _period = period;

            _updateLoopTimer = new Timer(appInfoState =>
            {
                if (!Monitor.TryEnter(_loopSync))
                {
                    return;
                }

                try
                {
                    if (_stoped)
                    {
                        return;
                    }

                    var appInfo0 = (IAppInfo) appInfoState;
                    ServerVersionInfo serverVersionInfo;

                    if (!appUpdater.CheckUpdates(appInfo0, out serverVersionInfo))
                    {
                        return;
                    }

                    newVersionFoundCallback(serverVersionInfo);
                }
                finally
                {
                    Monitor.Exit(_loopSync);
                }
            }, appInfo, _period, _period);
        }

        public void Start()
        {
            lock (_updateLoopTimerLock)
            {
                _updateLoopTimer.Change(_period, _period);
                _stoped = false;
            }
        }

        public void Stop()
        {
            lock (_updateLoopTimerLock)
            {
                _updateLoopTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _stoped = true;
            }
        }

        public void Dispose()
        {
            _updateLoopTimer.Dispose();
            _updateLoopTimer = null;
        }
    }
}