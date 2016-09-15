using System;

namespace Wilbit.AppUpdate.Configuration
{
    public interface ILogger
    {
        void Error(Exception exception);
        void Error(string message, Exception exception);
    }
}