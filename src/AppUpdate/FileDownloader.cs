using System;
using System.IO;
using System.Net;
using Wilbit.AppUpdate.Exceptions;

namespace Wilbit.AppUpdate
{
    public sealed class FileDownloader
    {
        private const int Timeout = 5 * 60 * 1000;
        public void DownloadFile(Uri sourceUri, string destinationFileLocation, Action<ProgressInfo> progressDelegate)
        {
            try
            {
                var request = WebRequest.Create(sourceUri);

                request.Timeout = Timeout;

                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var destFileStream = File.Create(destinationFileLocation))
                {
                    if (responseStream == null)
                    {
                        return;
                    }

                    var fileSize = response.ContentLength;
                    long totalBytesDownloaded = 0;
                    var buffer = new byte[BufferSize];
                    const int reportInterval = 1;
                    var stamp = DateTime.Now.Subtract(new TimeSpan(0, 0, reportInterval));
                    int bytesRead;
                    do
                    {
                        bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                        totalBytesDownloaded += bytesRead;
                        destFileStream.Write(buffer, 0, bytesRead);

                        if (progressDelegate == null || !(DateTime.Now.Subtract(stamp).TotalSeconds >= reportInterval))
                        {
                            continue;
                        }

                        ReportProgress(progressDelegate, totalBytesDownloaded, fileSize);
                        stamp = DateTime.Now;
                    } while (bytesRead > 0);// && !UpdateManager.Instance.ShouldStop);

                    ReportProgress(progressDelegate, totalBytesDownloaded, fileSize);

                    //return totalBytes == fileSize;
                }
            }
            catch (Exception e)
            {
                throw new FileDownloaderException($"Failed to download file \"{sourceUri}\" to \"{destinationFileLocation}\"", e);
            }
        }

        private static void ReportProgress(Action<ProgressInfo> action, long totalBytes, long fileSize)
        {
            if (action == null)
            {
                return;
            }

            var info = new ProgressInfo(value: totalBytes, minimum: 0, maximum: fileSize);

            action(info);
        }

        private const int BufferSize = 1024 * 100;
    }
}