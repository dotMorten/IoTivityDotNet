using System;
using System.Collections.Generic;
using System.Text;

namespace IotivityDotNet.Interop
{
    public enum OCMode
    {
        /// <summary>
        /// Client-only mode
        /// </summary>
        OC_CLIENT = 0,
        /// <summary>
        /// Server-only mode
        /// </summary>
        OC_SERVER,
        /// <summary>
        /// Client and server mode
        /// </summary>
        OC_CLIENT_SERVER,
        /// <summary>
        /// Client server mode along with routing capabilities
        /// </summary>
        OC_GATEWAY
    }

    /// <summary>
    /// Enum layout assumes some targets have 16-bit integer (e.g., Arduino).
    /// </summary>
    [Flags]
    public enum OCTransportFlags
    {
        /// <summary> default flag is</summary>
        OC_DEFAULT_FLAGS = 0,

        /** Insecure transport is the default (subject to change).*/
        /** secure the transport path*/
        OC_FLAG_SECURE = (1 << 4),

        /** IPv4 & IPv6 auto-selection is the default.*/
        /** IP & TCP adapter only.*/
        OC_IP_USE_V6 = (1 << 5),

        /** IP & TCP adapter only.*/
        OC_IP_USE_V4 = (1 << 6),

        /** Multicast only.*/
        OC_MULTICAST = (1 << 7),

        /** Link-Local multicast is the default multicast scope for IPv6.
         *  These are placed here to correspond to the IPv6 multicast address bits.*/

        /** IPv6 Interface-Local scope (loopback).*/
        OC_SCOPE_INTERFACE = 0x1,

        /** IPv6 Link-Local scope (default).*/
        OC_SCOPE_LINK = 0x2,

        /** IPv6 Realm-Local scope. */
        OC_SCOPE_REALM = 0x3,

        /** IPv6 Admin-Local scope. */
        OC_SCOPE_ADMIN = 0x4,

        /** IPv6 Site-Local scope. */
        OC_SCOPE_SITE = 0x5,

        /** IPv6 Organization-Local scope. */
        OC_SCOPE_ORG = 0x8,

        /**IPv6 Global scope. */
        OC_SCOPE_GLOBAL = 0xE,
    }

    /// <summary>
    /// Resource Properties.
    /// The value of a policy property is defined as bitmap.
    /// The LSB represents OC_DISCOVERABLE and Second LSB bit represents OC_OBSERVABLE and so on.
    /// Not including the policy property is equivalent to zero.
    /// </summary>
    [Flags]
    public enum OCResourceProperty : byte
    {
        /** When none of the bits are set, the resource is non-discoverable &
         *  non-observable by the client.*/
        OC_RES_PROP_NONE = (0),

        /** When this bit is set, the resource is allowed to be discovered by clients.*/
        OC_DISCOVERABLE = (1 << 0),

        /** When this bit is set, the resource is allowed to be observed by clients.*/
        OC_OBSERVABLE = (1 << 1),

        /** When this bit is set, the resource is initialized, otherwise the resource
         *  is 'inactive'. 'inactive' signifies that the resource has been marked for
         *  deletion or is already deleted.*/
        OC_ACTIVE = (1 << 2),

        /** When this bit is set, the resource has been marked as 'slow'.
         * 'slow' signifies that responses from this resource can expect delays in
         *  processing its requests from clients.*/
        OC_SLOW = (1 << 3),

#if __WITH_DTLS__ || __WITH_TLS__
    /** When this bit is set, the resource is a secure resource.*/
    OC_SECURE        = (1 << 4),
#else
        OC_SECURE = (0),
#endif

        /** When this bit is set, the resource is allowed to be discovered only
         *  if discovery request contains an explicit querystring.
         *  Ex: GET /oic/res?rt=oic.sec.acl */
        OC_EXPLICIT_DISCOVERABLE = (1 << 5)

# if WITH_MQ
        /** When this bit is set, the resource is allowed to be published */
        , OC_MQ_PUBLISHER = (1 << 6)
#endif

# if MQ_BROKER
        /** When this bit is set, the resource is allowed to be notified as MQ broker.*/
        , OC_MQ_BROKER = (1 << 7)
#endif
    }
    
    /// <summary>
    /// Declares Stack Results & Errors.
    /// </summary>
    public enum OCStackResult
    {
        /** Success status code - START HERE.*/
        OC_STACK_OK = 0,                /** 203, 205*/
        OC_STACK_RESOURCE_CREATED,      /** 201*/
        OC_STACK_RESOURCE_DELETED,      /** 202*/
        OC_STACK_CONTINUE,
        OC_STACK_RESOURCE_CHANGED,      /** 204*/
        /** Success status code - END HERE.*/

        /** Error status code - START HERE.*/
        OC_STACK_INVALID_URI = 20,
        OC_STACK_INVALID_QUERY,         /** 400*/
        OC_STACK_INVALID_IP,
        OC_STACK_INVALID_PORT,
        OC_STACK_INVALID_CALLBACK,
        OC_STACK_INVALID_METHOD,

        /** Invalid parameter.*/
        OC_STACK_INVALID_PARAM,
        OC_STACK_INVALID_OBSERVE_PARAM,
        OC_STACK_NO_MEMORY,
        OC_STACK_COMM_ERROR,            /** 504*/
        OC_STACK_TIMEOUT,
        OC_STACK_ADAPTER_NOT_ENABLED,
        OC_STACK_NOTIMPL,

        /** Resource not found.*/
        OC_STACK_NO_RESOURCE,           /** 404*/

        /** e.g: not supported method or interface.*/
        OC_STACK_RESOURCE_ERROR,
        OC_STACK_SLOW_RESOURCE,
        OC_STACK_DUPLICATE_REQUEST,

        /** Resource has no registered observers.*/
        OC_STACK_NO_OBSERVERS,
        OC_STACK_OBSERVER_NOT_FOUND,
        OC_STACK_VIRTUAL_DO_NOT_HANDLE,
        OC_STACK_INVALID_OPTION,        /** 402*/

        /** The remote reply contained malformed data.*/
        OC_STACK_MALFORMED_RESPONSE,
        OC_STACK_PERSISTENT_BUFFER_REQUIRED,
        OC_STACK_INVALID_REQUEST_HANDLE,
        OC_STACK_INVALID_DEVICE_INFO,
        OC_STACK_INVALID_JSON,

        /** Request is not authorized by Resource Server. */
        OC_STACK_UNAUTHORIZED_REQ,      /** 401*/
        OC_STACK_TOO_LARGE_REQ,         /** 413*/

        /** Error code from PDM */
        OC_STACK_PDM_IS_NOT_INITIALIZED,
        OC_STACK_DUPLICATE_UUID,
        OC_STACK_INCONSISTENT_DB,

        /**
         * Error code from OTM
         * This error is pushed from DTLS interface when handshake failure happens
         */
        OC_STACK_AUTHENTICATION_FAILURE,
        OC_STACK_NOT_ALLOWED_OXM,

        /** Request come from endpoint which is not mapped to the resource. */
        OC_STACK_BAD_ENDPOINT,

        /** Insert all new error codes here!.*/
#if WITH_PRESENCE
        OC_STACK_PRESENCE_STOPPED = 128,
        OC_STACK_PRESENCE_TIMEOUT,
        OC_STACK_PRESENCE_DO_NOT_HANDLE,
#endif

        /** Request is denied by the user*/
        OC_STACK_USER_DENIED_REQ,

        /** ERROR code from server */
        OC_STACK_FORBIDDEN_REQ,          /** 403*/
        OC_STACK_INTERNAL_SERVER_ERROR,  /** 500*/

        /** ERROR in stack.*/
        OC_STACK_ERROR = 255
        /** Error status code - END HERE.*/
    }


    /// <summary>
    /// Possible returned values from entity handler.
    /// </summary>
    public enum OCEntityHandlerResult
    {
        OC_EH_OK = 0,
        OC_EH_ERROR,
        OC_EH_SLOW,
        OC_EH_RESOURCE_CREATED = 201,
        OC_EH_RESOURCE_DELETED = 202,
        OC_EH_VALID = 203,
        OC_EH_CHANGED = 204,
        OC_EH_CONTENT = 205,
        OC_EH_BAD_REQ = 400,
        OC_EH_UNAUTHORIZED_REQ = 401,
        OC_EH_BAD_OPT = 402,
        OC_EH_FORBIDDEN = 403,
        OC_EH_RESOURCE_NOT_FOUND = 404,
        OC_EH_METHOD_NOT_ALLOWED = 405,
        OC_EH_NOT_ACCEPTABLE = 406,
        OC_EH_TOO_LARGE = 413,
        OC_EH_UNSUPPORTED_MEDIA_TYPE = 415,
        OC_EH_INTERNAL_SERVER_ERROR = 500,
        OC_EH_BAD_GATEWAY = 502,
        OC_EH_SERVICE_UNAVAILABLE = 503,
        OC_EH_RETRANSMIT_TIMEOUT = 504
    }



    /// <summary>
    /// OCDoResource methods to dispatch the request
    /// </summary>
    public enum OCMethod
    {
        OC_REST_NOMETHOD = 0,

        /** Read.*/
        OC_REST_GET = (1 << 0),

        /** Write.*/
        OC_REST_PUT = (1 << 1),

        /** Update.*/
        OC_REST_POST = (1 << 2),

        /** Delete.*/
        OC_REST_DELETE = (1 << 3),

        /** Register observe request for most up date notifications ONLY.*/
        OC_REST_OBSERVE = (1 << 4),

        /** Register observe request for all notifications, including stale notifications.*/
        OC_REST_OBSERVE_ALL = (1 << 5),

#if WITH_PRESENCE
        /** Subscribe for all presence notifications of a particular resource.*/
        OC_REST_PRESENCE = (1 << 7),

#endif
        /** Allows OCDoResource caller to do discovery.*/
        OC_REST_DISCOVER = (1 << 8)
    }

    /// <summary>
    /// These enums (OCTransportAdapter and OCTransportFlags) must
    /// be kept synchronized with OCConnectivityType(below) as well as
    /// CATransportAdapter and CATransportFlags(in CACommon.h).
    /// </summary>
    public enum OCTransportAdapter
    {
        /** value zero indicates discovery.*/
        OC_DEFAULT_ADAPTER = 0,

        /** IPv4 and IPv6, including 6LoWPAN.*/
        OC_ADAPTER_IP = (1 << 0),

        /** GATT over Bluetooth LE.*/
        OC_ADAPTER_GATT_BTLE = (1 << 1),

        /** RFCOMM over Bluetooth EDR.*/
        OC_ADAPTER_RFCOMM_BTEDR = (1 << 2),
#if RA_ADAPTER
        /**Remote Access over XMPP.*/
        OC_ADAPTER_REMOTE_ACCESS = (1 << 3),
#endif
        /** CoAP over TCP.*/
        OC_ADAPTER_TCP = (1 << 4),

        /** NFC Transport for Messaging.*/
        OC_ADAPTER_NFC = (1 << 5)
    }


    /// <summary>
    /// This enum type includes elements of both ::OCTransportAdapter and ::OCTransportFlags.
    /// It is defined conditionally because the smaller definition limits expandability on 32/64 bit
    /// integer machines, and the larger definition won't fit into an enum on 16-bit integer machines
    /// like Arduino.
    ///
    /// This structure must directly correspond to::OCTransportAdapter and::OCTransportFlags.
    /// </summary>
    public enum OCConnectivityType
    {
        /** use when defaults are ok. */
        CT_DEFAULT = 0,

        /** IPv4 and IPv6, including 6LoWPAN.*/
        CT_ADAPTER_IP = (1 << 16),

        /** GATT over Bluetooth LE.*/
        CT_ADAPTER_GATT_BTLE = (1 << 17),

        /** RFCOMM over Bluetooth EDR.*/
        CT_ADAPTER_RFCOMM_BTEDR = (1 << 18),

#if RA_ADAPTER
        /** Remote Access over XMPP.*/
        CT_ADAPTER_REMOTE_ACCESS = (1 << 19),
#endif
        /** CoAP over TCP.*/
        CT_ADAPTER_TCP = (1 << 20),

        /** NFC Transport.*/
        CT_ADAPTER_NFC = (1 << 21),

        /** Insecure transport is the default (subject to change).*/

        /** secure the transport path.*/
        CT_FLAG_SECURE = (1 << 4),

        /** IPv4 & IPv6 autoselection is the default.*/

        /** IP adapter only.*/
        CT_IP_USE_V6 = (1 << 5),

        /** IP adapter only.*/
        CT_IP_USE_V4 = (1 << 6),

        /** Link-Local multicast is the default multicast scope for IPv6.
         * These are placed here to correspond to the IPv6 address bits.*/

        /** IPv6 Interface-Local scope(loopback).*/
        CT_SCOPE_INTERFACE = 0x1,

        /** IPv6 Link-Local scope (default).*/
        CT_SCOPE_LINK = 0x2,

        /** IPv6 Realm-Local scope.*/
        CT_SCOPE_REALM = 0x3,

        /** IPv6 Admin-Local scope.*/
        CT_SCOPE_ADMIN = 0x4,

        /** IPv6 Site-Local scope.*/
        CT_SCOPE_SITE = 0x5,

        /** IPv6 Organization-Local scope.*/
        CT_SCOPE_ORG = 0x8,

        /** IPv6 Global scope.*/
        CT_SCOPE_GLOBAL = 0xE,
    }


    /// <summary>
    /// Quality of Service attempts to abstract the guarantees provided by the underlying transport
    /// protocol. The precise definitions of each quality of service level depend on the
    /// implementation. In descriptions below are for the current implementation and may changed
    /// over time.
    /// </summary>
    public enum OCQualityOfService
    {
        /** Packet delivery is best effort.*/
        OC_LOW_QOS = 0,

        /** Packet delivery is best effort.*/
        OC_MEDIUM_QOS,

        /** Acknowledgments are used to confirm delivery.*/
        OC_HIGH_QOS,

        /** No Quality is defined, let the stack decide.*/
        OC_NA_QOS
    }

    /// <summary>
    /// Entity's state
    /// </summary>
    public enum OCEntityHandlerFlag
    {
        /** Request state.*/
        OC_REQUEST_FLAG = (1 << 1),
        /** Observe state.*/
        OC_OBSERVE_FLAG = (1 << 2)
    }

    /// <summary>
    /// Possible return values from client application callback
    ///
    /// A client application callback returns an OCStackApplicationResult to indicate whether
    /// the stack should continue to keep the callback registered.
    /// </summary>
    public enum OCStackApplicationResult
    {
        /** Make no more calls to the callback and call the OCClientContextDeleter for this callback */
        OC_STACK_DELETE_TRANSACTION = 0,
        /** Keep this callback registered and call it if an apropriate event occurs */
        OC_STACK_KEEP_TRANSACTION
    }


    /// <summary>
    /// Action associated with observation.
    /// </summary>
    public enum OCObserveAction
    {
        /** To Register. */
        OC_OBSERVE_REGISTER = 0,

        /** To Deregister. */
        OC_OBSERVE_DEREGISTER = 1,

        /** Others. */
        OC_OBSERVE_NO_OPTION = 2,
    }


    /// <summary>
    /// Transport Protocol IDs.
    /// </summary>
    public enum OCTransportProtocolID
    {
        /** For invalid ID.*/
        OC_INVALID_ID = (1 << 0),

        /* For coap ID.*/
        OC_COAP_ID = (1 << 1)
    }

    /// <summary>
    /// Enum to describe the type of object held by the OCPayload object.
    /// </summary>
    public enum OCPayloadType
    {
        /** Contents of the payload are invalid */
        PAYLOAD_TYPE_INVALID,
        /** The payload is an OCDiscoveryPayload */
        PAYLOAD_TYPE_DISCOVERY,
        /** The payload of the device */
        PAYLOAD_TYPE_DEVICE,
        /** The payload type of the platform */
        PAYLOAD_TYPE_PLATFORM,
        /** The payload is an OCRepPayload */
        PAYLOAD_TYPE_REPRESENTATION,
        /** The payload is an OCSecurityPayload */
        PAYLOAD_TYPE_SECURITY,
        /** The payload is an OCPresencePayload */
        PAYLOAD_TYPE_PRESENCE
    }

    /// <summary>
    /// This enum type for indicate Transport Protocol Suites
    /// </summary>
    [Flags]
    public enum OCTpsSchemeFlags
    {
        /** For initialize */
        OC_NO_TPS = 0,

        /** coap + udp */
        OC_COAP = 1,

        /** coaps + udp */
        OC_COAPS = (1 << 1),

#if TCP_ADAPTER
        /** coap + tcp */
        OC_COAP_TCP = (1 << 2),

        /** coaps + tcp */
        OC_COAPS_TCP = (1 << 3),
#endif
#if HTTP_ADAPTER
        /** http + tcp */
        OC_HTTP = (1 << 4),

        /** https + tcp */
        OC_HTTPS = (1 << 5),
#endif
#if EDR_ADAPTER
        /** coap + rfcomm */
        OC_COAP_RFCOMM = (1 << 6),
#endif
        /** Allow all endpoint.*/
        OC_ALL = 0xffff
    }

}
