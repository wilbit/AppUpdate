using System.Diagnostics;

namespace Wilbit.AppUpdate.Helpers
{
    public static class ShellHelper
    {
        public static void ExecuteScript(string scriptLocation)
        {
            using (var process = new Process())
            {
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = scriptLocation,
                    UseShellExecute = true
                };
                process.StartInfo = startInfo;
                process.Start();
            }
        }
    }
}