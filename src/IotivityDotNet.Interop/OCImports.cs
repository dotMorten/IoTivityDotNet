namespace IotivityDotNet.Interop
{
    internal static class Constants
    {
#if __ANDROID__
        internal const string DLL_IMPORT_TARGET = "liboctbstack.so";
#else
        internal const string DLL_IMPORT_TARGET = "octbstack.dll";
#endif
    }
}