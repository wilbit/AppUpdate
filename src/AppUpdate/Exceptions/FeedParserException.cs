using System;

namespace Wilbit.AppUpdate.Exceptions
{
    public class FeedParserException : AppUpdateException
    {
        public FeedParserException(string message) : base(message)
        {
        }

        public FeedParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}