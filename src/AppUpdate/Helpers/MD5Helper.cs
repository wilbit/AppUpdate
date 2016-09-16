using System;
using System.IO;
using System.Security.Cryptography;

namespace Wilbit.AppUpdate.Helpers
{
    // ReSharper disable once InconsistentNaming
    public static class MD5Helper
    {
        public static string GetHashForFile(string fileLocation)
        {
            if (string.IsNullOrWhiteSpace(fileLocation)) throw new ArgumentNullException(nameof(fileLocation));

            using (var fileStream = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            using (var md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(fileStream)).Replace("-", string.Empty);
            }
        }
    }
}