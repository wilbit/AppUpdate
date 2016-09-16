using System;
using Wilbit.AppUpdate.Exceptions;
using Wilbit.AppUpdate.Helpers;

namespace Wilbit.AppUpdate
{
    public sealed class HashInfo
    {
        internal HashInfo(string algo, string value)
        {
            Algo = algo;
            Value = value;
        }

        public string Algo { get; }
        public string Value { get; }

        public void CheckHashForFileWithThrowing(string fileLocation)
        {
            if (!CheckHashForFile(fileLocation))
            {
                throw new HashInfoException($"Invalid hash for file \"{fileLocation}\"");
            }
        }

        public bool CheckHashForFile(string fileLocation)
        {
            if (fileLocation == null) throw new ArgumentNullException(nameof(fileLocation));

            if (Algo.Equals("MD5", StringComparison.OrdinalIgnoreCase))
            {
                var hash = MD5Helper.GetHashForFile(fileLocation);
                return string.Equals(hash, Value, StringComparison.OrdinalIgnoreCase);
            }

            throw new NotSupportedException($"Not supported hash algorithm \"{Algo}\"");
        }
    }
}