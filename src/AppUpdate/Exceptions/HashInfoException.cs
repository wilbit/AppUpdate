using System;

namespace Wilbit.AppUpdate.Exceptions
{
    public class HashInfoException : AppUpdateException
    {
        public HashInfoException(string message) : base(message)
        {
        }

        public HashInfoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}