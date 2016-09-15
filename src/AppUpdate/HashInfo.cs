using System;
using System.IO;
using System.Security.Cryptography;
using Wilbit.AppUpdate.Exceptions;

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
                using (var fileStream = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(fileStream);
                    var hashAsString = BitConverter.ToString(hash).Replace("-", string.Empty);

                    return string.Equals(hashAsString, Value, StringComparison.OrdinalIgnoreCase);
                }
            }

            throw new NotSupportedException($"Not supported hash algorithm \"{Algo}\"");
        }
    }
}