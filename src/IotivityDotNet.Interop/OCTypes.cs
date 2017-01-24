using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OCPayloadPtr = System.IntPtr;

namespace IotivityDotNet.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate OCStackApplicationResult OCClientResponseHandler(IntPtr context, IntPtr handle, OCClientResponse clientResponse);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate OCEntityHandlerResult OCEntityHandler(OCEntityHandlerFlag flag, OCEntityHandlerRequest entityHandlerRequest, IntPtr callbackParam);
    
    /// <summary>
    /// used inside a discovery payload
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCResourcePayload
    {
        public string uri;
        /// <summary>
        /// Type: OCStringLL
        /// </summary>
        public IntPtr types;
        /// <summary>
        /// Type: OCStringLL
        /// </summary>
        public IntPtr interfaces;
        public byte bitmap;
        public bool secure;
        public UInt16 port;
#if TCP_ADAPTER
        public UInt16 tcpPort;
#endif
        /// <summary>
        /// Type: OCResourcePayload
        /// </summary>
        public IntPtr next;
        /// <summary>
        /// Type: OCEndpointPayload
        /// </summary>
        public IntPtr eps;
    }
    /// <summary>
    /// used inside a discovery payload
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCRepPayload
    {
        /// <summary>
        /// Type: OCPayload
        /// </summary>
        public OCPayloadPtr basePayload;
        public string uri;
        /// <summary>
        /// Type: OCStringLL
        /// </summary>
        public IntPtr types;
        /// <summary>
        /// Type: OCStringLL
        /// </summary>
        public IntPtr interfaces;
        /// <summary>
        /// Type: OCRepPayloadValue
        /// </summary>
        public IntPtr values;
        /// <summary>
        /// Type: OCRepPayload
        /// </summary>
        public IntPtr next;

        public IEnumerable<string> Types
        {
            get
            {
                var ptr = types;
                if (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure<OCStringLL>(ptr);
                    return resource.Values;
                }
                return new string[] { };
            }
        }
        public IEnumerable<string> Interfaces
        {
            get
            {
                var ptr = interfaces;
                if (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure<OCStringLL>(ptr);
                    return resource.Values;
                }
                return new string[] { };
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OCSecurityPayload
    {
        public OCPayloadPtr basePayload;
        // [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
        // public byte[] securityData;
        public IntPtr securityData;
        public byte[] SecurityData
        {
            get
            {
                byte[] b = new byte[(int)payloadSize];
                Marshal.Copy(securityData, b, 0, b.Length);
                return b;
            }
        }
        public UIntPtr payloadSize;
    }

    /// <summary>
    /// Represents an enumerable collection of strings, each instance pointing to the <see cref="next"/> string instance.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCStringLL : IEnumerable<string>
    {
        public static IntPtr Create(IEnumerable<string> values)
        {
            if (values == null) return IntPtr.Zero;
            IntPtr next = IntPtr.Zero;
            OCStringLL nextInstance = null;
            IntPtr ptr = IntPtr.Zero;
            foreach(var item in values.Reverse())
            {
                ptr = Marshal.AllocCoTaskMem(IntPtr.Size * 2);
                Marshal.WriteIntPtr(ptr, next);
                Marshal.WriteIntPtr(ptr, IntPtr.Size, Marshal.StringToCoTaskMemAnsi(item));
                nextInstance = new OCStringLL() { value = item, next = next };
                next = ptr;
            }
            return ptr;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        /// <summary>
        /// Type: OCStringLL
        /// </summary>
        public IntPtr next;

        public string value;

        public OCStringLL Next
        {
            get
            {
                if (next == IntPtr.Zero) return null;
                return Marshal.PtrToStructure<OCStringLL>(next);
            }
        }

        public IEnumerable<string> Values
        {
            get
            {
                yield return value;
                var ptr = next;
                while (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure<OCStringLL>(ptr);
                    yield return resource.value;
                    ptr = resource.next;
                }
            }
        }
    }

    /// <summary>
    /// Incoming requests handled by the server. Requests are passed in as a parameter to the
    /// OCEntityHandler callback API.
    /// The OCEntityHandler callback API must be implemented in the application in order
    /// to receive these requests.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCEntityHandlerRequest
    {
        /// <summary>
        /// Associated resource. (Type: OCResourceHandle)
        /// </summary>
        public IntPtr resource;

        /// <summary>
        /// Associated request handle. (Type: OCRequestHandle)
        /// </summary>
        public IntPtr requestHandle;

        /// <summary>
        /// the REST method retrieved from received request PDU.
        /// </summary>
        public OCMethod method;

        /// <summary>
        /// description of endpoint that sent the request.
        /// </summary>
        public OCDevAddr devAddr;

        /// <summary>
        /// resource query send by client.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string query;

        /// <summary>
        /// Information associated with observation - valid only when OCEntityHandler flag includes ::OC_OBSERVE_FLAG.
        /// </summary>
        public OCObservationInfo obsInfo;

        /// <summary>
        /// Number of the received vendor specific header options.
        /// </summary>
        public byte numRcvdVendorSpecificHeaderOptions;

        /// <summary>
        /// Pointer to the array of the received vendor specific header options. (type: OCHeaderOption[])
        /// </summary>
        public IntPtr rcvdVendorSpecificHeaderOptions; 

        /// <summary>
        /// Message id.
        /// </summary>
        public UInt16 messageID;

        /// <summary>
        /// the payload from the request PDU.
        /// </summary>
        public IntPtr payload; //OCPayload

    }

    /// <summary>
    /// Possible returned values from entity handler.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCObservationInfo
    {
        /// <summary>
        /// Action associated with observation request.
        /// </summary>
        OCObserveAction action;

        /// <summary>
        /// Identifier for observation being registered/deregistered.
        /// </summary>
        byte obsId;
    }
    
    /// <summary>
    /// This structure will be used to define the vendor specific header options to be included
    /// in communication packets.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCHeaderOption
    {

        /// <summary>
        /// The protocol ID this option applies to.
        /// </summary>
        public OCTransportProtocolID protocolID;

        /// <summary>
        /// The header option ID which will be added to communication packets.
        /// </summary>
        public UInt16 optionID;

        /// <summary>
        /// its length 191.
        /// </summary>
        public UInt16 optionLength;

        /// <summary>
        /// pointer to its data.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] optionData;

#if SUPPORTS_DEFAULT_CTOR
            OCHeaderOption() = default;
    OCHeaderOption(OCTransportProtocolID pid,
                   uint16_t optId,
                   uint16_t optlen,
                           const uint8_t* optData)
        : protocolID(pid),
          optionID(optId),
          optionLength(optlen)
            {

                // parameter includes the null terminator.
                optionLength = optionLength < MAX_HEADER_OPTION_DATA_LENGTH ?
                                optionLength : MAX_HEADER_OPTION_DATA_LENGTH;
                memcpy(optionData, optData, optionLength);
                optionData[optionLength - 1] = '\0';
            }
#endif
    }

    /// <summary>
    /// End point identity.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCIdentity
    {
        /// <summary>
        /// Identity Length
        /// </summary>
        public UInt16 id_length;

        /// <summary>
        /// Array of end point identity.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 37)]
        public string id;
    }

    /// <summary>
    /// Response from queries to remote servers. Queries are made by calling the OCDoResource API.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCClientResponse
    {
        /// <summary>
        /// Address of remote server.
        /// </summary>
        public OCDevAddr devAddr;

        /// <summary>
        /// backward compatibility (points to devAddr).
        /// </summary>
        private IntPtr addr;

        /// <summary>
        /// backward compatibility.
        /// </summary>
        public OCConnectivityType connType;

        /// <summary>
        /// the security identity of the remote server.
        /// </summary>
        public OCIdentity identity;

        /// <summary>
        /// the is the result of our stack, OCStackResult should contain coap/other error codes.
        /// </summary>
        public OCStackResult result;

        /// <summary>
        /// If associated with observe, this will represent the sequence of notifications from server.
        /// </summary>
        public UInt32 sequenceNumber;

        /// <summary>
        /// resourceURI.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string resourceUri;

        /// <summary>
        /// the payload for the response PDU.
        /// </summary>
        public IntPtr payload;

        /// <summary>
        /// Number of the received vendor specific header options.
        /// </summary>
        public byte numRcvdVendorSpecificHeaderOptions;

        /// <summary>
        /// An array of the received vendor specific header options. Type: OCHeaderOption[], Size:50
        /// </summary>
        public IntPtr rcvdVendorSpecificHeaderOptions;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OCCallbackData
    {
        /// <summary>
        /// Pointer to the context.
        /// </summary>
        public IntPtr context;

        /// <summary>
        /// The pointer to a function the stack will call to handle the requests.
        /// </summary>
        public OCClientResponseHandler cb;

        /// <summary>
        /// A pointer to a function to delete the context when this callback is removed.
        /// </summary>
        public OCClientContextDeleter cd;

#if SUPPORTS_DEFAULT_CTOR
            OCCallbackData() = default;
            OCCallbackData(void* ctx, OCClientResponseHandler callback, OCClientContextDeleter deleter)
                :context(ctx), cb(callback), cd(deleter) { }
#endif
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OCClientContextDeleter
    {
        /// <summary>
        /// Pointer to the context.
        /// </summary>
        public IntPtr context;
    }

    /// <summary>
    /// Data structure to encapsulate IPv4/IPv6/Contiki/lwIP device addresses.
    /// OCDevAddr must be the same as CAEndpoint (in CACommon.h).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCDevAddr
    {
        /// <summary>
        /// adapter type.
        /// </summary>
        public OCTransportAdapter adapter;

        /// <summary>
        /// transport modifiers.
        /// </summary>
        public OCTransportFlags flags;

        /// <summary>
        /// for IP.
        /// </summary>
        public UInt16 port;

        /// <summary>
        /// address for all adapters.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 66)]
        public string addr;

        /// <summary>
        /// usually zero for default interface.
        /// </summary>
        public UInt32 ifindex;

        /// <summary>
        /// destination GatewayID:ClientId.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 66)]
        public string routeData;

        /// <summary>
        /// destination DeviceID.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 37)]
        public string deviceId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OCPayload
    {
        /// <summary>
        /// The type of message that was received
        /// </summary>
        public OCPayloadType type;
    }

    /// <summary>
    /// Request handle is passed to server via the entity handler for each incoming request.
    /// Stack assigns when request is received, server sets to indicate what request response is for.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCEntityHandlerResponse
    {
        /// <summary>
        /// Request handle (OCRequestHandle).
        /// </summary>
        public IntPtr requestHandle;

        /// <summary>
        /// Resource handle (OCResourceHandle).
        /// </summary>
        public IntPtr resourceHandle;

        /// <summary>
        /// Allow the entity handler to pass a result with the response.
        /// </summary>
        public OCEntityHandlerResult ehResult;

        /// <summary>
        /// This is the pointer to server payload data to be transferred.
        /// </summary>
        public IntPtr payload;

        /// <summary>
        /// number of the vendor specific header options.
        /// </summary>
        public byte numSendVendorSpecificHeaderOptions;

        /// <summary>
        /// An array of the vendor specific header options the entity handler wishes to use in response.
        /// OCHeaderOption sendVendorSpecificHeaderOptions[MAX_HEADER_OPTIONS];
        /// </summary>
        public IntPtr sendVendorSpecificHeaderOptions;

        /// <summary>
        /// URI of new resource that entity handler might create.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string resourceUri;

        /// <summary>
        /// Server sets to true for persistent response buffer, false for non-persistent response buffer
        /// </summary>
        public byte persistentBufferFlag;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OCUUIdentity
    {
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]
        public byte[] deviceId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OCRepPayloadValue
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string name;
        
        public OCRepPayloadPropType type;

        //public OCRepPayloadValueUnion value;
        // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        // public byte[] data;
        //MarshalAs(UnmanagedType.Struct, SizeConst = 8)]
        public OCRepPayloadValueUnion value;
        //union
        //{
        //    int64_t i;
        //    double d;
        //    bool b;
        //    char* str;

        //    /** ByteString object.*/
        //    OCByteString ocByteStr;

        //    struct OCRepPayload* obj;
        //    OCRepPayloadValueArray arr;
        //};

        public IntPtr next;
    }
    [StructLayout(LayoutKind.Explicit)] // 32bit: Size 24. 64bit: Size 36
    public struct OCRepPayloadValueUnion
    {
        [FieldOffset(0)]
        public Int64 i;

        [FieldOffset(0)]
        public double d;
        
        [FieldOffset(0)]
        public bool b;
         
        // [FieldOffset(0)]
        // [MarshalAs(UnmanagedType.LPStr)]
        // public string str;
        // 
        [FieldOffset(0)]
        public IntPtr ocByteStr;
        
        [FieldOffset(0)]
        public OCRepPayloadValueArray arr;
    }

    public struct OCRepPayloadValueArray
    {
        public OCRepPayloadPropType type;
        public UIntPtr dimensions0;
        //public UIntPtr dimensions1;
        //public UIntPtr dimensions2;

        //public IntPtr V1;
         public IntPtr V2;
         public IntPtr V3;
        public IntPtr value; //OCRepPayloadValueArrayUnion

    }
    [StructLayout(LayoutKind.Explicit)]
    public struct OCRepPayloadValueArrayUnion
    {
        [FieldOffset(0)]
        public long[] iArray;
        [FieldOffset(0)]
        public double[] dArray;
        [FieldOffset(0)]
        public bool[] bArray;
        [FieldOffset(0)]
        public string[] strArray;

        /** pointer to ByteString array.*/
        //[FieldOffset(0)]
        //IntPtr ocByteStrArray; //OCByteString

        //[FieldOffset(0)]
        //IntPtr[] objArray;
    }

    public enum OCRepPayloadPropType
    {
        OCREP_PROP_NULL,
        OCREP_PROP_INT,
        OCREP_PROP_DOUBLE,
        OCREP_PROP_BOOL,
        OCREP_PROP_STRING,
        OCREP_PROP_BYTE_STRING,
        OCREP_PROP_OBJECT,
        OCREP_PROP_ARRAY
    }

    /// <summary>
    /// This structure is expected as input for device properties.
    /// device name is mandatory and expected from the application
    /// device id of type UUID will be generated by the stack.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCDeviceInfo
    {
        /// <summary>
        /// Pointer to the device name
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string deviceName;
        /// <summary>
        /// Pointer to the types.
        /// </summary>
        public IntPtr types;
        /// <summary>
        /// Pointer to the device specification version.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string specVersion;
        /// <summary>
        /// Pointer to the device data model versions (in CSV format).
        /// </summary>
        public IntPtr dataModelVersions;
    }



    /// <summary>
    /// Persistent storage handlers. An APP must provide OCPersistentStorage handler pointers
    /// when it calls OCRegisterPersistentStorageHandler.
    /// Persistent storage open handler points to default file path.
    /// It should check file path and whether the file is symbolic link or no.
    /// Application can point to appropriate SVR database path for it's IoTivity Server.
    /// </summary>
    //[StructLayout(LayoutKind.Sequential)]
    //public class OCPersistentStorage
    //{
    //    /** Persistent storage file path.*/
    //    public FileOpenDelegate Open;

    //    /** Persistent storage read handler.*/
    //    public FileReadDelegate Read;

    //    /** Persistent storage write handler.*/
    //    public FileWriteDelegate Write;

    //    /** Persistent storage close handler.*/
    //    public FileCloseDelegate Close;

    //    /** Persistent storage unlink handler.*/
    //    public FileUnlinkDelegate Unlink;
    //}
    [StructLayout(LayoutKind.Sequential)]
    public class OCPersistentStorage
    {
        /** Persistent storage file path.*/
        public IntPtr Open;

        /** Persistent storage read handler.*/
        public IntPtr Read;

        /** Persistent storage write handler.*/
        public IntPtr Write;

        /** Persistent storage close handler.*/
        public IntPtr Close;

        /** Persistent storage unlink handler.*/
        public IntPtr Unlink;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr FileOpenDelegate([MarshalAs(UnmanagedType.LPStr)]string path, [MarshalAs(UnmanagedType.LPStr)]string mode); // FILE* (* open)(const char* path, const char* mode);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate UIntPtr FileReadDelegate(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr file); // size_t(* read)(void* ptr, size_t size, size_t nmemb, FILE * stream);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate UIntPtr FileWriteDelegate(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr file); // size_t(* write)(const void* ptr, size_t size, size_t nmemb, FILE * stream);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FileCloseDelegate(IntPtr file); // int (* close)(FILE* fp);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FileUnlinkDelegate([MarshalAs(UnmanagedType.LPStr)]string path); // int (* unlink)(const char* path);
}