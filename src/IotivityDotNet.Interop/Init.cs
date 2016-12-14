using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet.Interop
{
    internal static class Init
    {
        private static object initLock = new object();
        static bool isInitialized = false;
#if !__ANDROID__ && !NETFX_CORE
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetDllDirectory(string lpPathName);
#endif

        internal static void Initialize()
        {
            lock (initLock)
            {
                if (!isInitialized)
                {
                    try
                    {
#if !__ANDROID__ && !NETFX_CORE                        
                        bool is64bit = IntPtr.Size == 8;
                        bool ok = SetDllDirectory(is64bit ? "x64" : "x86"); //This is for .NET AnyCPU support. Returns false in UWP which is OK. Throws in Xamarin which we can ignore
#endif
                    }
                    catch
                    {
                    }
                    isInitialized = true;
                }
            }
        }

    }
}
