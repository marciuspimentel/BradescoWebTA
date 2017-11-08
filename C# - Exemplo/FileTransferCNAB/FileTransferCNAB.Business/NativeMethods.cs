using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferCNAB.Business
{
    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
