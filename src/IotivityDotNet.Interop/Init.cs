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
#if !__ANDROID__ && !NETFX_CORE && !__IOS__
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
#if !__ANDROID__ && !NETFX_CORE && !__IOS__
                        bool ok = true;
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            //This is for .NET AnyCPU support. Returns false in UWP which is OK. Throws in Xamarin which we can ignore
                            switch (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture)
                            {
                                case Architecture.X64:
                                    ok = SetDllDirectory("x64"); break;
                                case Architecture.X86:
                                    ok = SetDllDirectory("x86"); break;
                                default:
                                    break;
                            }
                        }
#endif
                    }
                    catch
                    {
                    }
                    isInitialized = true;
                    System.Diagnostics.Debug.WriteLine("************************ Interop initialized ************************");
                }
            }
        }

    }
}
