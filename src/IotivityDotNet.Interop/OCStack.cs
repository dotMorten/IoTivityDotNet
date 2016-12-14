using System;
using System.Runtime.InteropServices;

namespace IotivityDotNet.Interop
{
    /// <summary>
    /// P/invoke imports from iotivity\resource\csdk\stack\include\ocstack.h
    /// </summary>
    public static class OCStack
    {
        static OCStack()
        {
            Init.Initialize();
        }
        /// <summary>
        ///  This function Initializes the OC Stack.  Must be called prior to starting the stack.
        /// </summary>
        /// <param name="mode">OCMode Host device is client, server, or client-server.</param>
        /// <param name="serverFlags">OCTransportFlags Default server transport flags.</param>
        /// <param name="clientFlags">OCTransportFlags Default client transport flags.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCInit1(OCMode mode, OCTransportFlags serverFlags, OCTransportFlags clientFlags);

        /// <summary>
        /// This function Initializes the OC Stack.  Must be called prior to starting the stack.
        /// </summary>
        /// <param name="ipAddr">IP Address of host device. Deprecated parameter.</param>
        /// <param name="port">Port of host device. Deprecated parameter.</param>
        /// <param name="mode">OCMode Host device is client, server, or client-server.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCInit([MarshalAs(UnmanagedType.LPStr)] string ipAddr, UInt16 port, OCMode mode);

        /// <summary>
        /// This function Stops the OC stack.  Use for a controlled shutdown.
        /// </summary>
        /// <remarks>
        /// OCStop() performs operations similar to OCStopPresence(), as well as OCDeleteResource() on
        /// all resources this server is hosting. OCDeleteResource() performs operations similar to
        /// OCNotifyAllObservers() to notify all client observers that the respective resource is being
        /// deleted.
        /// </remarks>
        /// <returns>:OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCStop();

        /// <summary>
        /// This function starts receiving the multicast traffic. This can be only called
        /// when stack is in OC_STACK_INITIALIZED state but device is not receiving multicast
        /// traffic.
        /// </summary>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCStartMulticastServer();

        /// <summary>
        /// This function stops receiving the multicast traffic. The rest of the stack
        /// keeps working and no resource are deleted. Device can still receive the unicast
        /// traffic. Once this is set, no response to multicast /oic/res will be sent by the
        /// device. This is to be used for devices that uses other entity to push resources.
        /// </summary>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCStopMulticastServer();

        /// <summary>
        /// This function is Called in main loop of OC client or server.
        /// Allows low-level processing of stack services.
        /// </summary>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCProcess();

        /// <summary>
        /// This function discovers or Perform requests on a specified resource
        /// (specified by that Resource's respective URI).
        /// </summary>
        /// <remarks>
        /// Presence subscription amendments (i.e. adding additional resource type filters by calling
        /// this API again) require the use of the same base URI as the original request to successfully
        /// amend the presence filters.
        /// </remarks>
        /// <param name="handle">
        /// To refer to the request sent out on behalf of
        /// calling this API. This handle can be used to cancel this operation
        /// via the <see cref="OCCancel"/> API.
        /// @note: This reference is handled internally, and should not be free'd by
        /// the consumer.  A NULL handle is permitted in the event where the caller
        /// has no use for the return value.
        /// </param>
        /// <param name="method">To perform on the resource.</param>
        /// <param name="requestUri">URI of the resource to interact with. (Address prefix is deprecated in favor of destination.)</param>
        /// <param name="destination"Complete description of destination.></param>
        /// <param name="payload">Encoded request payload.</param>
        /// <param name="connectivityType">Modifier flags when destination is not given.</param>
        /// <param name="qos">
        /// Quality of service. Note that if this API is called on a uri with the
        /// well-known multicast IP address, the qos will be forced to ::OC_LOW_QOS
        /// since it is impractical to send other QOS levels on such addresses.
        /// </param>
        /// <param name="cbData">
        /// Asynchronous callback function that is invoked by the stack when
        /// discovery or resource interaction is received. The discovery could be
        /// related to filtered/scoped/particular resource. The callback is
        /// generated for each response received.
        /// </param>
        /// <param name="options">
        /// The address of an array containing the vendor specific header options
        /// to be sent with the request.
        /// </param>
        /// <param name="numOptions">Number of header options to be included.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCDoResource(out IntPtr handle,
                                   OCMethod method,
                           [MarshalAs(UnmanagedType.LPStr)] string requestUri,
                           OCDevAddr destination,
                           IntPtr payload, //OCPayload payload,
                           OCConnectivityType connectivityType,
                           OCQualityOfService qos,
                           OCCallbackData cbData,
                           OCHeaderOption options,
                           byte numOptions);

        /// <summary>
        /// This function cancels a request associated with a specific <see cref="OCDoResource"/> invocation.
        /// </summary>
        /// <param name="handle">Used to identify a specific OCDoResource invocation.</param>
        /// <param name="qos">Used to specify Quality of Service(read below).</param>
        /// <param name="options">Used to specify vendor specific header options when sending explicit observe cancellation.</param>
        /// <param name="numOptions">Number of header options to be included.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCCancel(IntPtr handle,
                               OCQualityOfService qos,
                               OCHeaderOption options,
                               byte numOptions);


        /**
        * Register Persistent storage callback.
        * @param   persistentStorageHandler  Pointers to open, read, write, close & unlink handlers.
        *
        * @return
        *     OC_STACK_OK                    No errors; Success.
        *     OC_STACK_INVALID_PARAM         Invalid parameter.
        */
        //  [DllImport(Constants.DLL_IMPORT_TARGET)]
        //  public static extern OCStackResult OCRegisterPersistentStorageHandler(OCPersistentStorage* persistentStorageHandler);

        /**
        * When operating in  OCServer or  OCClientServer mode,
        * this API will start sending out presence notifications to clients via multicast.
        * Once this API has been called with a success, clients may query for this server's presence and
        * this server's stack will respond via multicast.
        *
        * Server can call this function when it comes online for the first time, or when it comes back
        * online from offline mode, or when it re enters network.
        *
        * @param ttl         Time To Live in seconds.
        *                    @note: If ttl is '0', then the default stack value will be used (60 Seconds).
        *                    If ttl is greater than ::OC_MAX_PRESENCE_TTL_SECONDS, then the ttl will be
        *                    set to ::OC_MAX_PRESENCE_TTL_SECONDS.
        *
        * @return ::OC_STACK_OK on success, some other value upon failure.
        */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCStartPresence(UInt32 ttl);

        /**
         * When operating in OCServer or OCClientServer mode, this API will stop sending
         * out presence notifications to clients via multicast.
         * Once this API has been called with a success this server's stack will not respond to clients
         * querying for this server's presence.
         *
         * Server can call this function when it is terminating, going offline, or when going
         * away from network.
         *
         * @return ::OC_STACK_OK on success, some other value upon failure.
         */

        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCStopPresence();

        /// <summary>
        /// This function sets default device entity handler.
        /// </summary>
        /// <param name="entityHandler">
        /// Entity handler function that is called by ocstack to handle requests
        /// for any undefined resources or default actions.If NULL is passed it
        /// removes the device default entity handler.
        /// </param>
        /// <param name="callbackParameter"Parameter passed back when entityHandler is called.></param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCSetDefaultDeviceEntityHandler(OCEntityHandler entityHandler, IntPtr callbackParameter);

        /**
        * This function sets device information.
        *
        * Upon call to OCInit, the default Device Type (i.e. "rt") has already been set to the default
        * Device Type "oic.wk.d". You do not have to specify "oic.wk.d" in the OCDeviceInfo.types linked
        * list. The default Device Type is mandatory and always specified by this Device as the first
        * Device Type.
        *
        * @param deviceInfo   Structure passed by the server application containing the device
        *                     information.
        *
        * @return
        *     ::OC_STACK_OK               no errors.
        *     ::OC_STACK_INVALID_PARAM    invalid parameter.
        *     ::OC_STACK_ERROR            stack process error.
        */
        //OCStackResult OCSetDeviceInfo(OCDeviceInfo deviceInfo);

        /**
        * This function sets platform information.
        *
        * @param platformInfo   Structure passed by the server application containing
        *                       the platform information.
        *
        *
        * @return
        *     ::OC_STACK_OK               no errors.
        *     ::OC_STACK_INVALID_PARAM    invalid parameter.
        *     ::OC_STACK_ERROR            stack process error.
        */
        //OCStackResult OCSetPlatformInfo(OCPlatformInfo platformInfo);

        /// <summary>
        /// This function creates a resource.
        /// </summary>
        /// <param name="handle">Pointer to handle to newly created resource. Set by ocstack and used to refer to resource.</param>
        /// <param name="resourceTypeName">Name of resource type.  Example: "core.led".</param>
        /// <param name="resourceInterfaceName">Name of resource interface.  Example: "core.rw".</param>
        /// <param name="uri">URI of the resource.  Example:  "/a/led".</param>
        /// <param name="entityHandler">Entity handler function that is called by ocstack to handle requests, etc. NULL for default entity handler.</param>
        /// <param name="callbackParam">parameter passed back when entityHandler is called.</param>
        /// <param name="resourceProperties"> Properties supported by resource. Example: ::OC_DISCOVERABLE|::OC_OBSERVABLE.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCCreateResource([Out] out IntPtr handle,
                               [MarshalAs(UnmanagedType.LPStr)] string resourceTypeName,
                               [MarshalAs(UnmanagedType.LPStr)] string resourceInterfaceName,
                               [MarshalAs(UnmanagedType.LPStr)] string uri,
                               OCEntityHandler entityHandler,
                               IntPtr callbackParam,
                               OCResourceProperty resourceProperties);

        /// <summary>
        /// This function creates a resource.
        /// </summary>
        /// <remarks>
        /// Only supported TPS types on stack will be mapped to resource.
        /// It means "OC_COAPS" and "OC_COAPS_TCP" flags will be ignored if secure option
        /// not enabled on stack. Also "COAP_TCP" and "COAPS_TCP" flags will be ignored
        /// if stack does not support tcp mode.
        /// </remarks>
        /// <param name="handle">Pointer to handle to newly created resource. Set by ocstack and used to refer to resource.</param>
        /// <param name="resourceTypeName">Name of resource type.  Example: "core.led".</param>
        /// <param name="resourceInterfaceName">Name of resource interface.  Example: "core.rw".</param>
        /// <param name="uri">URI of the resource.  Example:  "/a/led".</param>
        /// <param name="entityHandler">Entity handler function that is called by ocstack to handle requests, etc. NULL for default entity handler.</param>
        /// <param name="callbackParam">parameter passed back when entityHandler is called.</param>
        /// <param name="resourceProperties"> Properties supported by resource. Example: ::OC_DISCOVERABLE|::OC_OBSERVABLE.</param>
        /// <param name="resourceTpsTypes">
        /// Transport Protocol Suites(TPS) types of resource for expose
        /// resource to specific transport adapter (e.g., TCP, UDP)
        /// with messaging protocol (e.g., COAP, COAPS).
        /// Example: "OC_COAP | OC_COAP_TCP"
        /// </param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCCreateResourceWithEp([Out] out IntPtr handle,
                                     [MarshalAs(UnmanagedType.LPStr)] string resourceTypeName,
                                     [MarshalAs(UnmanagedType.LPStr)] string resourceInterfaceName,
                                     [MarshalAs(UnmanagedType.LPStr)] string uri,
                                     OCEntityHandler entityHandler,
                                     IntPtr callbackParam,
                                     byte resourceProperties,
                                     OCTpsSchemeFlags resourceTpsTypes);

        /// <summary>
        /// This function returns flags of supported endpoint TPS on stack.
        /// </summary>
        /// <returns>Bit combinations of supported OCTpsSchemeFlags.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCTpsSchemeFlags OCGetSupportedEndpointTpsFlags();

        /// <summary>
        /// This function adds a resource to a collection resource. 
        /// </summary>
        /// <param name="collectionHandle"> Handle to the collection resource.</param>
        /// <param name="resourceHandle">Handle to resource to be added to the collection resource.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCBindResource(IntPtr collectionHandle, IntPtr resourceHandle);

        /// <summary>
        /// This function removes a resource from a collection resource.
        /// </summary>
        /// <param name="collectionHandle"> Handle to the collection resource.</param>
        /// <param name="resourceHandle">Handle to resource to be removed from the collection resource.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCUnBindResource(IntPtr collectionHandle, IntPtr resourceHandle);

        /**
         * This function binds a resource type to a resource.
         *
         * @param handle            Handle to the resource.
         * @param resourceTypeName  Name of resource type.  Example: "core.led".
         *
         * @return ::OC_STACK_OK on success, some other value upon failure.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCBindResourceTypeToResource(IntPtr handle, string resourceTypeName);

        /// <summary>
        /// This function binds a resource interface to a resource.
        /// </summary>
        /// <param name="handle">Handle to the resource.</param>
        /// <param name="resourceInterfaceName">Name of resource interface.  Example: "core.rw".</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCBindResourceInterfaceToResource(IntPtr handle, string resourceInterfaceName);

        //        /**
        //         * This function binds an entity handler to the resource.
        //         *
        //         * @param handle            Handle to the resource that the contained resource is to be bound.
        //         * @param entityHandler     Entity handler function that is called by ocstack to handle requests.
        //         * @param callbackParameter Context parameter that will be passed to entityHandler.
        //         *
        //         * @return ::OC_STACK_OK on success, some other value upon failure.
        //         */
        //        OCStackResult OCBindResourceHandler(OCResourceHandle handle,
        //                                            OCEntityHandler entityHandler,
        //                                            void* callbackParameter);
        //
        
        /// <summary>
        /// This function gets the number of resources that have been created in the stack.
        /// </summary>
        /// <param name="numResources">Pointer to count variable.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCGetNumberOfResources([Out] out byte numResources);
        
        /// <summary>
        /// This function gets a resource handle by index.
        /// </summary>
        /// <param name="index">Index of resource, 0 to Count - 1.</param>
        /// <returns> Found  resource handle or NULL if not found.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern IntPtr OCGetResourceHandle(byte index);

        /**
         * This function deletes resource specified by handle.  Deletes resource and all
         * resource type and resource interface linked lists.
         *
         * @note: OCDeleteResource() performs operations similar to OCNotifyAllObservers() to notify all
         * client observers that "this" resource is being deleted.
         *
         * @param handle          Handle of resource to be deleted.
         *
         * @return ::OC_STACK_OK on success, some other value upon failure.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCDeleteResource(IntPtr handle);

        /**
         * Get a string representation the server instance ID.
         * The memory is managed internal to this function, so freeing externally will result
         * in a runtime error.
         * Note: This will NOT seed the RNG, so it must be called after the RNG is seeded.
         * This is done automatically during the OCInit process,
         * so ensure that this call is done after that.
         *
         * @return A string representation  the server instance ID.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern string OCGetServerInstanceIDString();

        /**
         * This function gets the URI of the resource specified by handle.
         *
         * @param handle     Handle of resource.
         *
         * @return URI string if resource found or NULL if not found.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern string OCGetResourceUri(IntPtr handle);
        //
        //        /**
        //         * This function gets the properties of the resource specified by handle.
        //         *
        //         * @param handle                Handle of resource.
        //         *
        //         * @return OCResourceProperty   Bitmask or -1 if resource is not found.
        //         *
        //         * @note that after a resource is created, the OC_ACTIVE property is set for the resource by the
        //         * stack.
        //         */
        //        OCResourceProperty OCGetResourceProperties(OCResourceHandle handle);
        //
        /**
         * This function gets the number of resource types of the resource.
         *
         * @param handle            Handle of resource.
         * @param numResourceTypes  Pointer to count variable.
         *
         * @return ::OC_STACK_OK on success, some other value upon failure.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCGetNumberOfResourceTypes(IntPtr handle, [Out] out byte numResourceTypes);

        /**
         * This function gets name of resource type of the resource.
         *
         * @param handle       Handle of resource.
         * @param index        Index of resource, 0 to Count - 1.
         *
         * @return Resource type name if resource found or NULL if resource not found.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern string OCGetResourceTypeName(IntPtr handle, byte index);

        /**
         * This function gets the number of resource interfaces of the resource.
         *
         * @param handle                 Handle of resource.
         * @param numResourceInterfaces  Pointer to count variable.
         *
         * @return ::OC_STACK_OK on success, some other value upon failure.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCGetNumberOfResourceInterfaces(IntPtr handle,
                [Out] out byte numResourceInterfaces);

        /**
         * This function gets name of resource interface of the resource.
         *
         * @param handle      Handle of resource.
         * @param index       Index of resource, 0 to Count - 1.
         *
         * @return Resource interface name if resource found or NULL if resource not found.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern string OCGetResourceInterfaceName(IntPtr handle, byte index);
        //
        //        /**
        //         * This function gets methods of resource interface of the resource.
        //         *
        //         * @param handle      Handle of resource.
        //         * @param index       Index of resource, 0 to Count - 1.
        //         *
        //         * @return Allowed methods if resource found or NULL if resource not found.
        //         */
        //        uint8_t OCGetResourceInterfaceAllowedMethods(OCResourceHandle handle, uint8_t index);
        //
        //        /**
        //         * This function gets resource handle from the collection resource by index.
        //         *
        //         * @param collectionHandle   Handle of collection resource.
        //         * @param index              Index of contained resource, 0 to Count - 1.
        //         *
        //         * @return Handle to contained resource if resource found or NULL if resource not found.
        //         */
        //        OCResourceHandle OCGetResourceHandleFromCollection(OCResourceHandle collectionHandle,
        //                uint8_t index);
        //
        //        /**
        //         * This function gets the entity handler for a resource.
        //         *
        //         * @param handle            Handle of resource.
        //         *
        //         * @return Entity handler if resource found or NULL resource not found.
        //         */
        //        OCEntityHandler OCGetResourceHandler(OCResourceHandle handle);
        //
        /**
         * This function notify all registered observers that the resource representation has
         * changed. If observation includes a query the client is notified only if the query is valid after
         * the resource representation has changed.
         *
         * @param handle   Handle of resource.
         * @param qos      Desired quality of service for the observation notifications.
         *
         * @return ::OC_STACK_OK on success, some other value upon failure.
         */
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCNotifyAllObservers(IntPtr handle, OCQualityOfService qos);
        //
        //        /**
        //         * Notify specific observers with updated value of representation.
        //         * Before this API is invoked by entity handler it has finished processing
        //         * queries for the associated observers.
        //         *
        //         * @param handle                    Handle of resource.
        //         * @param obsIdList                 List of observation IDs that need to be notified.
        //         * @param numberOfIds               Number of observation IDs included in obsIdList.
        //         * @param payload                   Object representing the notification
        //         * @param qos                       Desired quality of service of the observation notifications.
        //         *
        //         * @note: The memory for obsIdList and payload is managed by the entity invoking the API.
        //         * The maximum size of the notification is 1015 bytes for non-Arduino platforms. For Arduino
        //         * the maximum size is 247 bytes.
        //         *
        //         * @return ::OC_STACK_OK on success, some other value upon failure.
        //         */
        //        OCStackResult
        //        OCNotifyListOfObservers(OCResourceHandle handle,
        //                                 OCObservationId* obsIdList,
        //                                 uint8_t numberOfIds,
        //                         const OCRepPayload* payload,
        //                         OCQualityOfService qos);
        //
        //

        /// <summary>
        /// This function sends a response to a request.
        /// The response can be a normal, slow, or block (i.e. a response that
        /// is too large to be sent in a single PDU and must span multiple transmissions).
        /// </summary>
        /// <param name="response">Pointer to structure that contains response parameters.</param>
        /// <returns>OC_STACK_OK on success, some other value upon failure.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCDoResponse(OCEntityHandlerResponse response);
        //
        //        //#ifdef DIRECT_PAIRING
        //        /**
        //         * The function is responsible for discovery of direct-pairing device is current subnet. It will list
        //         * all the device in subnet which support direct-pairing.
        //         * Caller must NOT free returned constant pointer
        //         *
        //         * @param[in] timeout Timeout in seconds, value till which function will listen to responses from
        //         *                    client before returning the list of devices.
        //         * @return OCDirectPairingDev_t pointer in case of success and NULL otherwise.
        //         */
        //        const OCDPDev_t* OCDiscoverDirectPairingDevices(unsigned short waittime);
        //
        //        /**
        //         * The function is responsible for return of paired device list via direct-pairing. It will list
        //         * all the device which is previousely paired with client.
        //         * Caller must NOT free returned constant pointer
        //         *
        //         * @return OCDirectPairingDev_t pointer in case of success and NULL otherwise.
        //         */
        //        const OCDPDev_t* OCGetDirectPairedDevices();
        //
        //        /**
        //         * The function is responsible for establishment of direct-pairing. It will proceed mode negotiation
        //         * and connect PIN based dtls session.
        //         *
        //         * @param[in] peer Target device to establish direct-pairing.
        //         * @param[in] pmSel Selected mode of pairing.
        //         * @param[in] pinNumber PIN number for authentication, pin lenght is defined DP_PIN_LENGTH(8).
        //         * @param[in] resultCallback Callback fucntion to event status of process.
        //         * @return OTM_SUCCESS in case of success and other value otherwise.
        //         */
        //        OCStackResult OCDoDirectPairing(void* ctx, OCDPDev_t* peer, OCPrm_t pmSel, char* pinNumber,
        //                                        OCDirectPairingCB resultCallback);
        //
        //        /**
        //         * This function sets uri being used for proxy.
        //         *
        //         * @param uri            NULL terminated resource uri for CoAP-HTTP Proxy.
        //         */
        //        OCStackResult OCSetProxyURI(const char* uri);
        //
        //#if RD_CLIENT || RD_SERVER
        //        /**
        //         * This function binds an resource unique id to the resource.
        //         *
        //         * @param handle            Handle to the resource that the contained resource is to be bound.
        //         * @param ins               Unique ID for resource.
        //         *
        //         * @return ::OC_STACK_OK on success, some other value upon failure.
        //         */
        //        OCStackResult OCBindResourceInsToResource(OCResourceHandle handle, uint8_t ins);
        //
        //        /**
        //         * This function gets the resource unique id for a resource.
        //         *
        //         * @param handle            Handle of resource.
        //         * @param ins               Unique ID for resource.
        //         *
        //         * @return Ins if resource found or 0 resource not found.
        //         */
        //        OCStackResult OCGetResourceIns(OCResourceHandle handle, uint8_t *ins);
        //
        //#endif
        //
        //        /**
        //        * This function gets a resource handle by resource uri.
        //        *
        //        * @param uri   Uri of Resource to get Resource handle.
        //        *
        //        * @return Found  resource handle or NULL if not found.
        //*/
        //        OCResourceHandle OCGetResourceHandleAtUri(const char* uri);
        //
        //# ifdef RD_SERVER
        //        /**
        //        * Search the RD database for queries.
        //        *
        //        * @param interfaceType is the interface type that is queried.
        //        * @param resourceType is the resource type that is queried.
        //        * @param discPayload is NULL if no resource found or else OCDiscoveryPayload with the details
        //        * about the resource.
        //        *
        //        * @return ::OC_STACK_OK in case of success or else other value.
        //*/
        //        OCStackResult OCRDDatabaseCheckResources(const char* interfaceType, const char* resourceType,
        //            OCDiscoveryPayload *discPayload);
        //#endif
        ////#endif // DIRECT_PAIRING
        //
        //        /**
        //         *  Add a header option to the given header option array.
        //         *
        //         * @param ocHdrOpt            Pointer to existing options.
        //         * @param numOptions          Number of existing options.
        //         * @param optionID            COAP option ID.
        //         * @param optionData          Option data value.
        //         * @param optionDataLength    Size of Option data value.
        //         *
        //         * @return ::OC_STACK_OK on success and other value otherwise.
        //         */
        //        OCStackResult OCSetHeaderOption(OCHeaderOption* ocHdrOpt,
        //                  size_t* numOptions,
        //                  uint16_t optionID,
        //                  void* optionData,
        //                  size_t optionDataLength);
        //
        //        /**
        //         *  Get data value of the option with specified option ID from given header option array.
        //         *
        //         * @param ocHdrOpt            Pointer to existing options.
        //         * @param numOptions          Number of existing options.
        //         * @param optionID            COAP option ID.
        //         * @param optionData          Pointer to option data.
        //         * @param optionDataLength    Size of option data value.
        //         * @param receivedDatalLength Pointer to the actual length of received data.
        //         *
        //         * @return ::OC_STACK_OK on success and other value otherwise.
        //         */
        //        OCStackResult
        //        OCGetHeaderOption(OCHeaderOption* ocHdrOpt,
        //                          size_t numOptions,
        //                          uint16_t optionID,
        //                          void* optionData,
        //                          size_t optionDataLength,
        //                          uint16_t* receivedDatalLength);

        /// <summary>
        ///  gets the deviceId of the client
        /// </summary>
        /// <param name="deviceId">deviceId pointer.</param>
        /// <returns>:OC_STACK_OK if success.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCGetDeviceId(out OCUUIdentity deviceId);

        /// <summary>
        /// sets the deviceId of the client
        /// </summary>
        /// <param name="deviceId">deviceId pointer.</param>
        /// <returns>OC_STACK_OK if success.</returns>
        [DllImport(Constants.DLL_IMPORT_TARGET)]
        public static extern OCStackResult OCSetDeviceId(OCUUIdentity deviceId);

        //        /**
        //         * Encode an address string to match RFC 6874.
        //         *
        //         * @param outputAddress    a char array to be written with the encoded string.
        //         *
        //         * @param outputSize       size of outputAddress buffer.
        //         *
        //         * @param inputAddress     a char array of size <= CA_MAX_URI_LENGTH
        //         *                         containing a valid IPv6 address string.
        //         *
        //         * @return ::OC_STACK_OK on success and other value otherwise.
        //         */
        //        OCStackResult OCEncodeAddressForRFC6874(char* outputAddress,
        //                                                size_t outputSize,
        //                                        const char* inputAddress);
        //
        //        /**
        //         * Decode an address string according to RFC 6874.
        //         *
        //         * @param outputAddress    a char array to be written with the decoded string.
        //         *
        //         * @param outputSize       size of outputAddress buffer.
        //         *
        //         * @param inputAddress     a valid percent-encoded address string.
        //         *
        //         * @param end              NULL if the entire entire inputAddress is a null-terminated percent-
        //         *                         encoded address string.  Otherwise, a pointer to the first byte that
        //         *                         is not part of the address string (e.g., ']' in a URI).
        //         *
        //         * @return ::OC_STACK_OK on success and other value otherwise.
        //         */
        //        OCStackResult OCDecodeAddressForRFC6874(char* outputAddress,
        //                                                size_t outputSize,
        //                                        const char* inputAddress,
        //                                        const char* end);
        //
        //        /**
        //         * Set the value of /oic/d and /oic/p properties. This function is a generic function that sets for
        //         * all OCF defined properties.
        //         *
        //         * @param type the payload type for device and platform as defined in @ref OCPayloadType.
        //         * @param propName the pre-defined property as per OCF spec.
        //         * @param value the value of the property to be set.
        //         *
        //         * @return ::OC_STACK_OK on success and other value otherwise.
        //         */
        //        OCStackResult OCSetPropertyValue(OCPayloadType type, const char* propName, const void* value);
        //
        //        /**
        //         * Get the value of /oic/d and /oic/p properties. This function is a generic function that get value
        //         * for all OCF defined properties.
        //         *
        //         * @param type the payload type for device and platform as defined in @ref OCPayloadType.
        //         * @param propName the pre-defined as per OCF spec.
        //         * @param value this holds the return value.  In case of error will be set to NULL.
        //         *
        //         * @return ::OC_STACK_OK on success and other value otherwise.
        //         */
        //        OCStackResult OCGetPropertyValue(OCPayloadType type, const char* propName, void** value);
        //

    }
}