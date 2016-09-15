using System;

namespace Wilbit.AppUpdate.Exceptions
{
    public class FileDownloaderException : AppUpdateException
    {
        public FileDownloaderException(string message) : base(message)
        {
        }

        public FileDownloaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}