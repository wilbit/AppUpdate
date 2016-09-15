using System;

namespace Wilbit.AppUpdate.Configuration
{
    public interface IAppInfo
    {
        string AppLocation { get; }
        string AppFileName { get; }
        string AppParams { get; }
        Version Version { get; }
    }
}