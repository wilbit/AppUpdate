using System;

namespace Wilbit.AppUpdate.Exceptions
{
    public class FeedSourceException : AppUpdateException
    {
        public FeedSourceException(string message) : base(message)
        {
        }

        public FeedSourceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}