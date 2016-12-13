using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate OCStackApplicationResult OCClientResponseHandler(IntPtr context, IntPtr handle, OCClientResponse clientResponse);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate OCEntityHandlerResult OCEntityHandler(OCEntityHandlerFlag flag, OCEntityHandlerRequest entityHandlerRequest, IntPtr callbackParam);
    
    /// <summary>
    /// used inside a discovery payload
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCResourcePayload
    {
        public string uri;
        public IntPtr types; //OCStringLL
        public IntPtr interfaces; //OCStringLL
        public byte bitmap;
        public bool secure;
        public UInt16 port;
#if TCP_ADAPTER
        public UInt16 tcpPort;
#endif
        public IntPtr next; //OCResourcePayload
        public IntPtr eps; //OCEndpointPayload
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OCStringLL
    {
        public IntPtr next; //OCStringLL
        public string value;
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
        /** Associated resource.*/
        public IntPtr resource; //OCResourceHandle

        /** Associated request handle.*/
        public IntPtr requestHandle; //OCRequestHandle

        /** the REST method retrieved from received request PDU.*/
        public OCMethod method;

        /** description of endpoint that sent the request.*/
        public OCDevAddr devAddr;

        /** resource query send by client.*/
        [MarshalAs(UnmanagedType.LPStr)]
        public string query;

        /** Information associated with observation - valid only when OCEntityHandler flag includes ::OC_OBSERVE_FLAG.*/
        public OCObservationInfo obsInfo;

        /** Number of the received vendor specific header options.*/
        public byte numRcvdVendorSpecificHeaderOptions;

        /** Pointer to the array of the received vendor specific header options.*/
        public OCHeaderOption rcvdVendorSpecificHeaderOptions;

        /** Message id.*/
        public UInt16 messageID;

        /** the payload from the request PDU.*/
        public OCPayload payload;

    }

    /**
      * Possible returned values from entity handler.
      */
    [StructLayout(LayoutKind.Sequential)]
    public class OCObservationInfo
    {
        /** Action associated with observation request.*/
        OCObserveAction action;

        /** Identifier for observation being registered/deregistered.*/
        byte obsId;
    }
    
    /// <summary>
    /// This structure will be used to define the vendor specific header options to be included
    /// in communication packets.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class OCHeaderOption
    {

        /** The protocol ID this option applies to.*/
        public OCTransportProtocolID protocolID;

        /** The header option ID which will be added to communication packets.*/
        public UInt16 optionID;

        /** its length 191.*/
        public UInt16 optionLength;

        /** pointer to its data.*/
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
        /** Address of remote server.*/
        public OCDevAddr devAddr; //

        /** backward compatibility (points to devAddr).*/
        private IntPtr addr;

        /** backward compatibility.*/
        public OCConnectivityType connType;

        /** the security identity of the remote server.*/
        public OCIdentity identity;

        /** the is the result of our stack, OCStackResult should contain coap/other error codes.*/
        public OCStackResult result;

        /** If associated with observe, this will represent the sequence of notifications from server.*/
        public UInt32 sequenceNumber;

        /** resourceURI.*/
        [MarshalAs(UnmanagedType.LPStr)]
        public string resourceUri;

        /** the payload for the response PDU.*/
        public IntPtr payload;

        /** Number of the received vendor specific header options.*/
        public byte numRcvdVendorSpecificHeaderOptions;

        /** An array of the received vendor specific header options.*/
        public IntPtr rcvdVendorSpecificHeaderOptions; //Type: OCHeaderOption[], Size:50
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
        /** adapter type.*/
        public OCTransportAdapter adapter;

        /** transport modifiers.*/
        public OCTransportFlags flags;

        /** for IP.*/
        public UInt16 port;

        /** address for all adapters.*/
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 66)]
        public string addr;

        /** usually zero for default interface.*/
        public UInt32 ifindex;

        /** destination GatewayID:ClientId.*/
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 66)]
        public string routeData;

        /** destination DeviceID.*/
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
    /**
    * Request handle is passed to server via the entity handler for each incoming request.
    * Stack assigns when request is received, server sets to indicate what request response is for.
*/

    [StructLayout(LayoutKind.Sequential)]
    public class OCEntityHandlerResponse
    {
        /** Request handle.*/
        public IntPtr requestHandle; //OCRequestHandle

        /** Resource handle.*/
        public IntPtr resourceHandle; //OCResourceHandle

        /** Allow the entity handler to pass a result with the response.*/
        public OCEntityHandlerResult ehResult;

        /** This is the pointer to server payload data to be transferred.*/
        public IntPtr payload;

        /** number of the vendor specific header options .*/
        public byte numSendVendorSpecificHeaderOptions;

        /** An array of the vendor specific header options the entity handler wishes to use in response.*/
        public IntPtr sendVendorSpecificHeaderOptions; // OCHeaderOption sendVendorSpecificHeaderOptions[MAX_HEADER_OPTIONS];

        /** URI of new resource that entity handler might create.*/
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string resourceUri;

        /** Server sets to true for persistent response buffer,false for non-persistent response buffer*/
        public byte persistentBufferFlag;
    }

    public class OCUUIdentity
    {
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]
        public byte[] deviceId;
    }
}