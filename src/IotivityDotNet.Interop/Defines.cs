using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet.Interop
{
    /// <summary>
    /// Defined constants
    /// </summary>
    public static class Defines
    {
        /**
         *  OIC Virtual resources supported by every OIC device.
         **/

        ///<summary>
        ///  Default discovery mechanism using '/oic/res' is supported by all OIC devices
        ///  That are Discoverable.
        /// </summary>
        public const string OC_RSRVD_WELL_KNOWN_URI = "/oic/res";

        /// <summary>Device URI.</summary>
        public const string OC_RSRVD_DEVICE_URI = "/oic/d";

        /// <summary>Platform URI.</summary>
        public const string OC_RSRVD_PLATFORM_URI = "/oic/p";

        /// <summary>Resource Type.</summary>
        public const string OC_RSRVD_RESOURCE_TYPES_URI = "/oic/res/types/d";

        /// <summary>Gateway URI.</summary>
        public const string OC_RSRVD_GATEWAY_URI = "/oic/gateway";

        /// <summary>MQ Broker URI.</summary>
        public const string OC_RSRVD_WELL_KNOWN_MQ_URI = "/oic/ps";

        /// <summary>KeepAlive URI.</summary>
        public const string OC_RSRVD_KEEPALIVE_URI = "/oic/ping";


        /** Presence  **/

        /// <summary>Presence URI through which the OIC devices advertise their presence.</summary>
        public const string OC_RSRVD_PRESENCE_URI = "/oic/ad";

        /// <summary>Presence URI through which the OIC devices advertise their device presence.</summary>
        public const string OC_RSRVD_DEVICE_PRESENCE_URI = "/oic/prs";

        /// <summary>Sets the default time to live (TTL) for presence.</summary>
        public const int OC_DEFAULT_PRESENCE_TTL_SECONDS = 60;

        /// <summary>For multicast Discovery mechanism.</summary>
        public const string OC_MULTICAST_DISCOVERY_URI = "/oic/res";

        /// <summary>Separator for multiple query string.</summary>
        public const string OC_QUERY_SEPARATOR = "&;";

        ///<summary>
        /// OC_MAX_PRESENCE_TTL_SECONDS sets the maximum time to live (TTL) for presence.
        /// NOTE: Changing the setting to a longer duration may lead to unsupported and untested
        /// operation.
        /// 60 sec/min * 60 min/hr * 24 hr/day
        /// </summary>
        public const int OC_MAX_PRESENCE_TTL_SECONDS = (60 * 60 * 24);


        ///<summary>
        ///  Presence "Announcement Triggers".
        /// </summary>

        /// <summary>To create.</summary>
        public const string OC_RSRVD_TRIGGER_CREATE = "create";

        /// <summary>To change.</summary>
        public const string OC_RSRVD_TRIGGER_CHANGE = "change";

        /// <summary>To delete.</summary>
        public const string OC_RSRVD_TRIGGER_DELETE = "delete";

        ///<summary>
        ///   Attributes used to form a proper OIC conforming JSON message.
        /// </summary>

        public const string OC_RSRVD_OC = "oic";


        /// <summary>For payload. </summary>

        public const string OC_RSRVD_PAYLOAD = "payload";

        /// <summary>To represent href </summary>
        public const string OC_RSRVD_HREF = "href";

        /// <summary>To represent property</summary>
        public const string OC_RSRVD_PROPERTY = "prop";

        /// <summary>For representation.</summary>
        public const string OC_RSRVD_REPRESENTATION = "rep";

        /// <summary>To represent content type.</summary>
        public const string OC_RSRVD_CONTENT_TYPE = "ct";

        /// <summary>To represent resource type.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE = "rt";

        /// <summary>To represent resource type with presence.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_PRESENCE = "oic.wk.ad";

        /// <summary>To represent resource type with device.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_DEVICE = "oic.wk.d";

        /// <summary>To represent resource type with platform.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_PLATFORM = "oic.wk.p";

        /// <summary>To represent resource type with collection.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_COLLECTION = "oic.wk.col";

        /// <summary>To represent resource type with RES.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_RES = "oic.wk.res";

        /// <summary>To represent content type with MQ Broker.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_MQ_BROKER = "oic.wk.ps";

        /// <summary>To represent content type with MQ Topic.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_MQ_TOPIC = "oic.wk.ps.topic";


        /// <summary>To represent interface.</summary>
        public const string OC_RSRVD_INTERFACE = "if";

        /// <summary>To indicate how long RD should publish this item.</summary>
        public const string OC_RSRVD_DEVICE_TTL = "lt";

        /// <summary>To represent time to live.</summary>
        public const string OC_RSRVD_TTL = "ttl";

        /// <summary>To represent non</summary>
        public const string OC_RSRVD_NONCE = "non";

        /// <summary>To represent trigger type.</summary>
        public const string OC_RSRVD_TRIGGER = "trg";

        /// <summary>To represent links.</summary>
        public const string OC_RSRVD_LINKS = "links";

        /// <summary>To represent default interface.</summary>
        public const string OC_RSRVD_INTERFACE_DEFAULT = "oic.if.baseline";

        /// <summary>To represent read-only interface.</summary>
        public const string OC_RSRVD_INTERFACE_READ = "oic.if.r";

        /// <summary>To represent read-write interface.</summary>
        public const string OC_RSRVD_INTERFACE_READ_WRITE = "oic.if.rw";

        /// <summary>To represent ll interface.</summary>
        public const string OC_RSRVD_INTERFACE_LL = "oic.if.ll";

        /// <summary>To represent batch interface.</summary>
        public const string OC_RSRVD_INTERFACE_BATCH = "oic.if.b";

        /// <summary>To represent actuator interface.</summary>
        public const string OC_RSRVD_INTERFACE_ACTUATOR = "oic.if.a";

        /// <summary>To represent sensor interface.</summary>
        public const string OC_RSRVD_INTERFACE_SENSOR = "oic.if.s";

        /// <summary>To represent interface group.</summary>
        public const string OC_RSRVD_INTERFACE_GROUP = "oic.mi.grp";

        /// <summary>To represent FW version.</summary>
        public const string OC_RSRVD_FW_VERSION = "mnfv";

        /// <summary>To represent host name.</summary>
        public const string OC_RSRVD_HOST_NAME = "hn";

        /// <summary>To represent policy.</summary>
        public const string OC_RSRVD_POLICY = "p";

        /// <summary>To represent bitmap.</summary>
        public const string OC_RSRVD_BITMAP = "bm";

        /// <summary>For security.</summary>
        public const string OC_RSRVD_SECURE = "sec";

        /// <summary>Port. </summary>
        public const string OC_RSRVD_HOSTING_PORT = "port";

        /// <summary>TCP Port. </summary>
        public const string OC_RSRVD_TCP_PORT = "tcp";

        /// <summary>TLS Port. </summary>
        public const string OC_RSRVD_TLS_PORT = "tls";

        /// <summary>For Server instance ID.</summary>
        public const string OC_RSRVD_SERVER_INSTANCE_ID = "sid";

        /// <summary>To represent endpoints.</summary>
        public const string OC_RSRVD_ENDPOINTS = "eps";

        /// <summary>To represent endpoint.</summary>
        public const string OC_RSRVD_ENDPOINT = "ep";

        /// <summary>To represent priority.</summary>
        public const string OC_RSRVD_PRIORITY = "pri";

        /**
         *  Platform.
         **/

        /// <summary>Platform ID. </summary>
        public const string OC_RSRVD_PLATFORM_ID = "pi";

        /// <summary>Platform MFG NAME. </summary>
        public const string OC_RSRVD_MFG_NAME = "mnmn";

        /// <summary>Platform URL. </summary>
        public const string OC_RSRVD_MFG_URL = "mnml";

        /// <summary>Model Number.</summary>
        public const string OC_RSRVD_MODEL_NUM = "mnmo";

        /// <summary>Platform MFG Date.</summary>
        public const string OC_RSRVD_MFG_DATE = "mndt";

        /// <summary>Platform versio.n </summary>
        public const string OC_RSRVD_PLATFORM_VERSION = "mnpv";

        /// <summary>Platform Operating system version. </summary>
        public const string OC_RSRVD_OS_VERSION = "mnos";

        /// <summary>Platform Hardware version. </summary>
        public const string OC_RSRVD_HARDWARE_VERSION = "mnhw";

        ///<summary>Platform Firmware version. </summary>
        public const string OC_RSRVD_FIRMWARE_VERSION = "mnfv";

        /// <summary>Support URL for the platform. </summary>
        public const string OC_RSRVD_SUPPORT_URL = "mnsl";

        /// <summary>System time for the platform. </summary>
        public const string OC_RSRVD_SYSTEM_TIME = "st";

        /// <summary>VID for the platform. </summary>
        public const string OC_RSRVD_VID = "vid";
        /**
         *  Device.
         **/

        /// <summary>Device ID.</summary>
        public const string OC_RSRVD_DEVICE_ID = "di";

        /// <summary>Device Name.</summary>
        public const string OC_RSRVD_DEVICE_NAME = "n";

        /// <summary>Device specification version.</summary>
        public const string OC_RSRVD_SPEC_VERSION = "icv";

/// <summary>Device data model.</summary>
        public const string OC_RSRVD_DATA_MODEL_VERSION = "dmv";

/// <summary>Device specification version.</summary>
        public const string OC_SPEC_VERSION = "core.1.1.0";

        /// <summary>Device Data Model version.</summary>
        public const string OC_DATA_MODEL_VERSION = "res.1.1.0,sh.1.1.0";

#if RA_ADAPTER
            /// <summary>Max Device address size. </summary>
            public const string MAX_ADDR_STR_SIZE (256)
#else
        /// <summary>Max Address could be
        /// "coaps+tcp://[xxxx:xxxx:xxxx:xxxx:xxxx:xxxx:yyy.yyy.yyy.yyy]:xxxxx"
        /// +1 for null terminator.
        /// </summary>
        public const int MAX_ADDR_STR_SIZE = (66);
#endif

        /// <summary>Length of MAC address </summary>
        public const int MAC_ADDR_STR_SIZE = (17);

        /// <summary>Blocks of MAC address </summary>
        public const int MAC_ADDR_BLOCKS = (6);

        /// <summary>Max identity size. </summary>
        public const int MAX_IDENTITY_SIZE = (37);

        /// <summary>Universal unique identity size. </summary>
        public const int UUID_IDENTITY_SIZE = (128 / 8);

        /// <summary>Resource Directory </summary>

        /// <summary>Resource Directory URI used to Discover RD and Publish resources.</summary>
        public const string OC_RSRVD_RD_URI = "/oic/rd";

        /// <summary>To represent resource type with rd.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_RD = "oic.wk.rd";

        /// <summary>RD Discovery bias factor type. </summary>
        public const string OC_RSRVD_RD_DISCOVERY_SEL = "sel";

        /// <summary>Resource URI used to discover Proxy </summary>
        public const string OC_RSRVD_PROXY_URI = "/oic/chp";

        /// <summary>Resource URI used to discover Proxy </summary>
        public const int OC_RSRVD_PROXY_OPTION_ID = 35;

        /// <summary>Base URI. </summary>
        public const string OC_RSRVD_BASE_URI = "baseURI";

        /// <summary>Unique value per collection/link. </summary>
        public const string OC_RSRVD_INS = "ins";

        /// <summary>Allowable resource types in the links. </summary>
        public const string OC_RSRVD_RTS = "rts";

        /// <summary>Default relationship. </summary>
        public const string OC_RSRVD_DREL = "drel";

        /// <summary>Defines relationship between links. </summary>
        public const string OC_RSRVD_REL = "rel";

        /// <summary>Defines title. </summary>
        public const string OC_RSRVD_TITLE = "title";

        /// <summary>Defines URI. </summary>
        public const string OC_RSRVD_URI = "anchor";

        /// <summary>Defines media type. </summary>
        public const string OC_RSRVD_MEDIA_TYPE = "type";

        /// <summary>To represent resource type with Publish RD.</summary>
        public const string OC_RSRVD_RESOURCE_TYPE_RDPUBLISH = "oic.wk.rdpub";

        /** Cloud Account **/

        /// <summary>Account URI.</summary>
        public const string OC_RSRVD_ACCOUNT_URI = "/oic/account";

        /// <summary>Account user URI.</summary>
        public const string OC_RSRVD_ACCOUNT_SEARCH_URI = "/oic/account/search";

        /// <summary>Account session URI.</summary>
        public const string OC_RSRVD_ACCOUNT_SESSION_URI = "/oic/account/session";

        /// <summary>Account token refresh URI.</summary>
        public const string OC_RSRVD_ACCOUNT_TOKEN_REFRESH_URI = "/oic/account/tokenrefresh";

        /// <summary>ACL group URI.</summary>
        public const string OC_RSRVD_ACL_GROUP_URI = "/oic/acl/group";

        /// <summary>ACL invite URI.</summary>
        public const string OC_RSRVD_ACL_INVITE_URI = "/oic/acl/invite";

        /// <summary>Defines auth provider. </summary>
        public const string OC_RSRVD_AUTHPROVIDER = "authprovider";

        /// <summary>Defines auth code. </summary>
        public const string OC_RSRVD_AUTHCODE = "authcode";

        /// <summary>Defines access token. </summary>
        public const string OC_RSRVD_ACCESS_TOKEN = "accesstoken";

        /// <summary>Defines login. </summary>
        public const string OC_RSRVD_LOGIN = "login";

        /// <summary>Defines search. </summary>
        public const string OC_RSRVD_SEARCH = "search";

        /// <summary>Defines grant type. </summary>
        public const string OC_RSRVD_GRANT_TYPE = "granttype";

        /// <summary>Defines refresh token. </summary>
        public const string OC_RSRVD_REFRESH_TOKEN = "refreshtoken";

        /// <summary>Defines user UUID. </summary>
        public const string OC_RSRVD_USER_UUID = "uid";

        /// <summary>Defines group ID. </summary>
        public const string OC_RSRVD_GROUP_ID = "gid";

        /// <summary>Defines member of group ID. </summary>
        public const string OC_RSRVD_MEMBER_ID = "mid";

        /// <summary>Defines invite. </summary>
        public const string OC_RSRVD_INVITE = "invite";

        /// <summary>Defines accept. </summary>
        public const string OC_RSRVD_ACCEPT = "accept";

        /// <summary>Defines operation. </summary>
        public const string OC_RSRVD_OPERATION = "op";

        /// <summary>Defines add. </summary>
        public const string OC_RSRVD_ADD = "add";

        /// <summary>Defines delete. </summary>
        public const string OC_RSRVD_DELETE = "delete";

        /// <summary>Defines owner. </summary>
        public const string OC_RSRVD_OWNER = "owner";

        /// <summary>Defines members. </summary>
        public const string OC_RSRVD_MEMBERS = "members";

        /// <summary>To represent grant type with refresh token. </summary>
        public const string OC_RSRVD_GRANT_TYPE_REFRESH_TOKEN = "refresh_token";

        /// <summary>Cloud CRL </summary>
        public const string OC_RSRVD_PROV_CRL_URL = "/oic/credprov/crl";

        public const string OC_RSRVD_LAST_UPDATE = "lu";

        public const string OC_RSRVD_THIS_UPDATE = "tu";

        public const string OC_RSRVD_NEXT_UPDATE = "nu";

        public const string OC_RSRVD_SERIAL_NUMBERS = "rcsn";

        public const string OC_RSRVD_CRL = "crl";

        public const string OC_RSRVD_CRL_ID = "crlid";

        /// <summary>Cloud ACL </summary>
        public const string OC_RSRVD_GROUP_URL = "/oic/group";

        public const string OC_RSRVD_ACL_GROUP_URL = "/oic/acl/group";

        public const string OC_RSRVD_ACL_INVITE_URL = "/oic/acl/invite";
        public const string OC_RSRVD_ACL_VERIFY_URL = "/oic/acl/verify";

        public const string OC_RSRVD_ACL_ID_URL = "/oic/acl/id";

        public const string OC_RSRVD_OWNER_ID = "oid";

        public const string OC_RSRVD_ACL_ID = "aclid";

        public const string OC_RSRVD_ACE_ID = "aceid";

        public const string OC_RSRVD_SUBJECT_ID = "sid";

        public const string OC_RSRVD_REQUEST_METHOD = "rm";

        public const string OC_RSRVD_REQUEST_URI = "uri";

        public const string OC_RSRVD_GROUP_MASTER_ID = "gmid";

        public const string OC_RSRVD_GROUP_TYPE = "gtype";

        public const string OC_RSRVD_SUBJECT_TYPE = "stype";

        public const string OC_RSRVD_GROUP_ID_LIST = "gidlist";

        public const string OC_RSRVD_MEMBER_ID_LIST = "midlist";

        public const string OC_RSRVD_DEVICE_ID_LIST = "dilist";

        public const string OC_RSRVD_ACCESS_CONTROL_LIST = "aclist";

        public const string OC_RSRVD_RESOURCES = "resources";

        public const string OC_RSRVD_VALIDITY = "validity";

        public const string OC_RSRVD_PERIOD = "period";

        public const string OC_RSRVD_RECURRENCE = "recurrence";

        public const string OC_RSRVD_INVITED = "invited";

        public const string OC_RSRVD_ENCODING = "encoding";

        public const string OC_OIC_SEC = "oic.sec";

        public const string OC_RSRVD_BASE64 = "base64";

        public const string OC_RSRVD_DER = "der";

        public const string OC_RSRVD_PEM = "pem";

        public const string OC_RSRVD_RAW = "raw";

        public const string OC_RSRVD_UNKNOWN = "unknown";

        public const string OC_RSRVD_DATA = "data";

        public const string OC_RSRVD_RESOURCE_OWNER_UUID = "rowneruuid";

        public const string OC_RSRVD_SUBJECT_UUID = "subjectuuid";

        public const string OC_RSRVD_PERMISSION_MASK = "permission";

        public const string OC_RSRVD_GROUP_PERMISSION = "gp";

        public const string OC_RSRVD_GROUP_ACL = "gacl";

        /// <summary>Certificete Sign Request </summary>
        public const string OC_RSRVD_PROV_CERT_URI = "/oic/credprov/cert";

        public const string OC_RSRVD_CSR = "csr";

        public const string OC_RSRVD_CERT = "cert";

        public const string OC_RSRVD_CACERT = "certchain";

        public const string OC_RSRVD_TOKEN_TYPE = "tokentype";

        public const string OC_RSRVD_EXPIRES_IN = "expiresin";

        public const string OC_RSRVD_REDIRECT_URI = "redirecturi";

        public const string OC_RSRVD_CERTIFICATE = "certificate";
    }
}
