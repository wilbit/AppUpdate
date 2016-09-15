using System;

namespace Wilbit.AppUpdate
{
    public sealed class ServerVersionInfo
    {
        internal ServerVersionInfo(string fileName, Version version, HashInfo hash)
        {
            FileName = fileName;
            Version = version;
            Hash = hash;
        }

        public string FileName { get; }
        public Version Version { get; }
        public HashInfo Hash { get; }
        public Feed Feed { get; internal set; }
    }
}