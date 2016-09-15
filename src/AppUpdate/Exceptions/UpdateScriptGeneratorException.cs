using System;

namespace Wilbit.AppUpdate.Exceptions
{
    public class UpdateScriptGeneratorException : AppUpdateException
    {
        public UpdateScriptGeneratorException(string message) : base(message)
        {
        }

        public UpdateScriptGeneratorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}