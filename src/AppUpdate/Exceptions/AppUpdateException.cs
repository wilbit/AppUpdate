using System;

namespace Wilbit.AppUpdate.Exceptions
{
    public abstract class AppUpdateException : ApplicationException
    {
        protected AppUpdateException(string message) : base(message)
        {
        }

        protected AppUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}