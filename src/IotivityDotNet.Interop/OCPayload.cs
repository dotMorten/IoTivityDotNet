using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//Using alias for pointers to easier see what type of pointer is returned
using OCPayloadPtr = System.IntPtr;
using OCDiscoveryPayloadPtr = System.IntPtr;
using OCRepPayloadPtr = System.IntPtr;
using OCResourcePayloadPtr = System.IntPtr;
using OCSecurityPayloadPtr = System.IntPtr;

namespace IotivityDotNet.Interop
{
    public static class OCPayloadInterop
    {
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern OCSecurityPayloadPtr OCSecurityPayloadCreate(byte[] securityData, UIntPtr size);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OCSecurityPayloadDestroy(OCSecurityPayloadPtr payload);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern OCDiscoveryPayloadPtr OCDiscoveryPayloadCreate();

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr OCDiscoveryPayloadGetResourceCount(OCDiscoveryPayloadPtr handle);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern OCResourcePayloadPtr OCDiscoveryPayloadGetResource(OCDiscoveryPayloadPtr handle, UIntPtr index);
        
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OCPayloadDestroy(OCPayloadPtr handle);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern OCRepPayloadPtr OCRepPayloadCreate();

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern OCRepPayloadPtr OCRepPayloadClone(OCRepPayloadPtr payload);
        
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OCRepPayloadAppend(OCRepPayloadPtr parent, IntPtr child);
        
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetUri(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string uri);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadAddResourceType(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string resourceType);

        //       bool OCRepPayloadAddInterface(OCRepPayloadPtr payload, const char* iface);
        //       bool OCRepPayloadAddModelVersion(OCRepPayloadPtr payload, const char* dmv);
        //
        //       bool OCRepPayloadAddResourceTypeAsOwner(OCRepPayloadPtr payload, char* resourceType);
        //       bool OCRepPayloadAddInterfaceAsOwner(OCRepPayloadPtr payload, char* iface);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadIsNull(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name);
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetNull(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetPropInt(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, long value);
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadGetPropInt(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, out long value);


        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetPropDouble(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)]  string name, double value);
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadGetPropDouble(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, out double value);

        //       /**
        //        * This function allocates memory for the byte string and sets it in the payload.
        //        *
        //        * @param payload      Pointer to the payload to which byte string needs to be added.
        //        * @param name         Name of the byte string.
        //        * @param value        Byte string and it's length.
        //        *
        //        * @return true on success, false upon failure.
        //        */
        //       bool OCRepPayloadSetPropByteString(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, OCByteString value);
        //
        //       /**
        //        * This function sets the byte string in the payload.
        //        *
        //        * @param payload      Pointer to the payload to which byte string needs to be added.
        //        * @param name         Name of the byte string.
        //        * @param value        Byte string and it's length.
        //        *
        //        * @return true on success, false upon failure.
        //        */
        //       bool OCRepPayloadSetPropByteStringAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //       OCByteString* value);
        //
        //       /**
        //        * This function gets the byte string from the payload.
        //        *
        //        * @param payload      Pointer to the payload from which byte string needs to be retrieved.
        //        * @param name         Name of the byte string.
        //        * @param value        Byte string and it's length.
        //        *
        //        * @note: Caller needs to invoke OCFree on value.bytes after it is finished using the byte string.
        //        *
        //        * @return true on success, false upon failure.
        //        */
        //       bool OCRepPayloadGetPropByteString(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //       OCByteString* value);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetPropString(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetPropStringAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadGetPropString(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, out IntPtr value);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetPropBool(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, bool value);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadGetPropBool(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)]  string name, out bool value);

        //       bool OCRepPayloadSetPropObject(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, const OCRepPayload* value);
        //       bool OCRepPayloadSetPropObjectAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, OCRepPayload* value);
        //       bool OCRepPayloadGetPropObject(const OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, OCRepPayload** value);
        //
        //       #ifdef __WITH_TLS__
        //       bool OCRepPayloadSetPropPubDataType(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, const OicSecKey_t* value);
        //               bool OCRepPayloadSetPropPubDataTypeAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, const OicSecKey_t* value);
        //               bool OCRepPayloadGetPropPubDataType(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name, OicSecKey_t *value);
        //       #endif
        //
        //   /**
        //    * This function allocates memory for the byte string array and sets it in the payload.
        //    *
        //    * @param payload      Pointer to the payload to which byte string array needs to be added.
        //    * @param name         Name of the byte string.
        //    * @param array        Byte string array.
        //    * @param dimensions   Number of byte strings in above array.
        //    *
        //    * @return true on success, false upon failure.
        //    */
        //   bool OCRepPayloadSetByteStringArrayAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //           OCByteString* array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);
        //
        //       /**
        //        * This function sets the byte string array in the payload.
        //        *
        //        * @param payload      Pointer to the payload to which byte string array needs to be added.
        //        * @param name         Name of the byte string.
        //        * @param array        Byte string array.
        //        * @param dimensions   Number of byte strings in above array.
        //        *
        //        * @return true on success, false upon failure.
        //        */
        //       bool OCRepPayloadSetByteStringArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //       const OCByteString* array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);
        //
        //       /**
        //        * This function gets the byte string array from the payload.
        //        *
        //        * @param payload      Pointer to the payload from which byte string array needs to be retrieved.
        //        * @param name         Name of the byte string array.
        //        * @param value        Byte string array.
        //        * @param dimensions   Number of byte strings in above array.
        //        *
        //        * @note: Caller needs to invoke OICFree on 'bytes' field of all array elements after it is
        //        *        finished using the byte string array.
        //        *
        //        * @return true on success, false upon failure.
        //        */
        //       bool OCRepPayloadGetByteStringArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               OCByteString** array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);
        //
        //               bool OCRepPayloadSetIntArrayAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //                       int64_t* array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetIntArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
                       long[] array, UIntPtr[] dimensions);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadGetIntArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
                       out long[] array, out UIntPtr[] dimensions);

        //       bool OCRepPayloadSetDoubleArrayAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               double* array, UIntPtr dimensions[MAX_REP_ARRAY_DEPTH]);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetDoubleArray(OCRepPayloadPtr payload, string name,
                       double[] array, UIntPtr[] dimensions);
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadGetDoubleArray(OCRepPayloadPtr payload, string name,
                       out double[] array, out UIntPtr[] dimensions);

        //       bool OCRepPayloadSetStringArrayAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               char** array, UIntPtr dimensions[MAX_REP_ARRAY_DEPTH]);
        //       bool OCRepPayloadSetStringArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               const char** array, UIntPtr dimensions[MAX_REP_ARRAY_DEPTH]);
        //       bool OCRepPayloadGetStringArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               char*** array, UIntPtr dimensions[MAX_REP_ARRAY_DEPTH]);
        //
        //       bool OCRepPayloadSetBoolArrayAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               bool* array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadSetBoolArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
                       bool[] array, UIntPtr[] dimensions);

        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OCRepPayloadGetBoolArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
                       out bool[] array, out UIntPtr[] dimensions);
        
        //       bool OCRepPayloadSetPropObjectArrayAsOwner(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               OCRepPayload** array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);
        //               bool OCRepPayloadSetPropObjectArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               const OCRepPayload** array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);
        //       bool OCRepPayloadGetPropObjectArray(OCRepPayloadPtr payload, [MarshalAs(UnmanagedType.LPStr)] string name,
        //               OCRepPayload*** array, size_t dimensions[MAX_REP_ARRAY_DEPTH]);
        
        [DllImport(Constants.DLL_IMPORT_TARGET, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OCRepPayloadDestroy(OCRepPayloadPtr payload);
    }
}
