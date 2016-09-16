using System;

namespace Wilbit.AppUpdate.Configuration
{
    public interface IAppUpdateLogger
    {
        void Error(Exception exception);
        void Error(string message, Exception exception);
    }
}