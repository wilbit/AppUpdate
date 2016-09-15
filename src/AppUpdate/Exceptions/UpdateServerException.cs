using System;

namespace Wilbit.AppUpdate.Exceptions
{
    public class UpdateServerException : AppUpdateException
    {
        public UpdateServerException(string message) : base(message)
        {
        }

        public UpdateServerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}