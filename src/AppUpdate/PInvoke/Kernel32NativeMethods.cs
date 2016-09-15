using System.Runtime.InteropServices;

namespace Wilbit.AppUpdate.PInvoke
{
    internal static class Kernel32NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern int GetSystemDefaultLCID();
    }
}