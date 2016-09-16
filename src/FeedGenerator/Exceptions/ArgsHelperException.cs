using System;

namespace Wilbit.FeedGenerator.Exceptions
{
    public class ArgsHelperException : ApplicationException
    {
        public ArgsHelperException(string message) : base(message)
        {
        }

        public ArgsHelperException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}