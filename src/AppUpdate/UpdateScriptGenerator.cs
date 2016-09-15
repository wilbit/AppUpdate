using System;
using System.Globalization;
using System.IO;
using System.Text;
using Wilbit.AppUpdate.Exceptions;
using Wilbit.AppUpdate.PInvoke;

namespace Wilbit.AppUpdate
{
    internal sealed class UpdateScriptGenerator
    {
        public string GenerateScript(string applicationDirectory, string applicationFileName, string applicationParams, string updateDirectory, string updateFileName)
        {
            const string scriptFileName = "update.cmd";
            var scriptPath = Path.Combine(updateDirectory, scriptFileName);

            try
            {
                var consoleEncoding = GetConsoleEncoding();
                var fileStream = new FileStream(scriptPath, FileMode.Create);
                using (var writer = new StreamWriter(fileStream, consoleEncoding))
                {
                    if (Path.IsPathRooted(updateDirectory))
                    {
                        var drive = $"{updateDirectory[0]}:";
                        writer.WriteLine(drive);
                    }
                    writer.WriteLine($"chcp {consoleEncoding.CodePage}");
                    writer.WriteLine($"cd \"{updateDirectory}\"");
                    writer.WriteLine($"\"{updateFileName}\" /DIR=\"{applicationDirectory}\" /VERYSILENT /CLOSEAPPLICATIONS /NORESTARTAPPLICATIONS");
                    writer.WriteLine($"del \"{updateFileName}\"");
                    writer.WriteLine($"cd \"{applicationDirectory}\"");
                    writer.WriteLine($"start /D\"{applicationDirectory}\" {applicationFileName} {applicationParams}");
                    writer.WriteLine($"cd \"{updateDirectory}\"");
                    writer.WriteLine($"del \"{scriptFileName}\"");
                }

                return scriptPath;
            }
            catch (Exception e)
            {
                throw new UpdateScriptGeneratorException($"Failed to generate script \"{scriptPath}\"", e);
            }
        }

        private static Encoding GetConsoleEncoding()
        {
            var lcid = Kernel32NativeMethods.GetSystemDefaultLCID();
            var cultureInfo = CultureInfo.GetCultureInfo(lcid);
            var codepage = cultureInfo.TextInfo.OEMCodePage;
            var encoding = Encoding.GetEncoding(codepage);

            return encoding;
        }
    }
}