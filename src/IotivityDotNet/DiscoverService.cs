using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class DiscoverResource
    {
        private string requestUri;
        private OCCallbackData cbData;
        private IntPtr handle;
        private GCHandle gchandle;
        private GCHandle gchandle2;
        OCClientResponseHandler onDiscoverHandler;
        public DiscoverResource(string requestUri = "/oic/res")
        {
            onDiscoverHandler = OnDiscover;
            gchandle2 = GCHandle.Alloc(onDiscoverHandler);
            cbData = new OCCallbackData() { cb = onDiscoverHandler };
            this.requestUri = requestUri;
            gchandle = GCHandle.Alloc(cbData);
        }

        ~DiscoverResource()
        {
            Stop();
            gchandle.Free();
            gchandle2.Free();
        }
        bool isRunning;

        public async void Start()
        {
            isRunning = true;
            if (handle != IntPtr.Zero)
                return;
            while (isRunning)
            {
                var ret = OCStack.OCDoResource(out handle, OCMethod.OC_REST_DISCOVER, requestUri, null, IntPtr.Zero, OCConnectivityType.CT_DEFAULT, OCQualityOfService.OC_LOW_QOS, cbData, null, 0);
                OCStackException.ThrowIfError(ret);
                await Task.Delay(5000);
                ret = OCStack.OCCancel(handle, OCQualityOfService.OC_LOW_QOS, null, 0);
                handle = IntPtr.Zero;
            }
        }

        public void Stop()
        {
            isRunning = false;
            if(handle != IntPtr.Zero)
            {
                var ret = OCStack.OCCancel(handle, OCQualityOfService.OC_LOW_QOS, null, 0);
                OCStackException.ThrowIfError(ret);
                handle = IntPtr.Zero;
            }
        }

        private OCStackApplicationResult OnDiscover(IntPtr context, IntPtr handle, OCClientResponse clientResponse)
        {
            if (clientResponse == null)
            {
                return OCStackApplicationResult.OC_STACK_DELETE_TRANSACTION;
            }
            var response = new ClientResponse<DiscoveryPayload>(clientResponse);
            var addr = response.DeviceAddress; // new DeviceAddress(clientResponse.devAddr);
            foreach (var item in response.Payload.Resources)
            {
                string key = $"{addr.Address}:{addr.Port}{item.Uri}";
                if (!discoveredResources.ContainsKey(key))
                {
                    discoveredResources[key] = DateTimeOffset.UtcNow;
                    ResourceDiscovered?.Invoke(this, new ResourceDiscoveredEventArgs(addr, item));
                }
            }
            return OCStackApplicationResult.OC_STACK_KEEP_TRANSACTION;
        }

        public event EventHandler<ResourceDiscoveredEventArgs> ResourceDiscovered;

        private Dictionary<string, DateTimeOffset> discoveredResources = new Dictionary<string, DateTimeOffset>();
    }

    public class ResourceDiscoveredEventArgs : EventArgs
    {
        internal ResourceDiscoveredEventArgs(DeviceAddress address, ResourcePayload payload)
        {
            Address = address;
            Payload = payload;
        }

        public DeviceAddress Address { get; }

        public string Uri => Payload.Uri;

        public ResourcePayload Payload { get; }
    }

    public class ClientResponseEventArgs<T> : EventArgs where T : Payload
    {
        internal ClientResponseEventArgs(OCClientResponse clientResponse, IntPtr handle)
        {
            Response = new ClientResponse<T>(clientResponse);
            Handle = handle;
        }

        public ClientResponse<T> Response { get; }

        public IntPtr Handle { get; }
    }

    public class ClientResponse<T> where T : Payload
    {
        private OCClientResponse _response;
        internal ClientResponse(OCClientResponse response)
        {
            _response = response;
        }

        private DeviceAddress _deviceAddress;

        public DeviceAddress DeviceAddress
        {
            get
            {
                return _deviceAddress ?? (_deviceAddress = new DeviceAddress(_response.devAddr));
            }
        }

        private T _payload;

        public T Payload
        {
            get
            {
                if (_payload == null)
                {
                    var pl = Marshal.PtrToStructure<OCPayload>(_response.payload);
                    //clientResponse.payload.
                    if (pl.type == OCPayloadType.PAYLOAD_TYPE_DISCOVERY && typeof(T) == typeof(DiscoveryPayload))
                    {
                        _payload = new DiscoveryPayload(_response.payload) as T;
                    }
                    else if (pl.type == OCPayloadType.PAYLOAD_TYPE_REPRESENTATION && typeof(T) == typeof(RepPayload))
                    {
                        _payload = new RepPayload(_response.payload) as T;
                    }
                    else
                        throw new NotImplementedException();
                }
                return _payload;

            }
        }

        public OCConnectivityType ConnectivityType => _response.connType;

        public string ResourceUri => _response.resourceUri;
    }
}
