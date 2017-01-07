using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class ResourceClient : IDisposable
    {
        private IntPtr _handle;
        private OCCallbackData _observeCallbackData;
        private OCClientResponseHandler _observeCallbackHandler;
        private string _resourceUri;
        private DeviceAddress _address;

        public ResourceClient(DeviceAddress address, string resourceUri)
        {
            _observeCallbackData = new OCCallbackData();
            _observeCallbackData.cb = _observeCallbackHandler = OnObserveCallback;
            _resourceUri = resourceUri;
            _address = address;
            //var result = OCStack.OCDoResource(out _handle, OCMethod.OC_REST_OBSERVE, resourceUri, address.OCDevAddr, IntPtr.Zero, OCConnectivityType.CT_DEFAULT, OCQualityOfService.OC_LOW_QOS, _observeCallbackData, null, 0);
            //OCStackException.ThrowIfError(result, "Failed to observe resource");
        }

        ~ResourceClient()
        {
            OCStack.OCDeleteResource(_handle);
        }

        public Task<ClientResponse<Payload>> PostAsync(string resourceTypeName, Dictionary<string, object> data)
        {
            return SendAsync(resourceTypeName, data, OCMethod.OC_REST_POST);
        }

        public Task<ClientResponse<Payload>> PutAsync(string resourceTypeName, Dictionary<string, object> data)
        {
            return SendAsync(resourceTypeName, data, OCMethod.OC_REST_PUT);
        }

        public Task<ClientResponse<Payload>> GetAsync(string resourceTypeName)
        {
            return SendAsync(resourceTypeName, null, OCMethod.OC_REST_GET);
        }

        private async Task<ClientResponse<Payload>> SendAsync(string resourceTypeName, Dictionary<string, object> data, OCMethod method)
        {
            var tcs = new TaskCompletionSource<ClientResponse<Payload>>();
            var callbackData = new OCCallbackData();
            OCClientResponseHandler handler = (context, handle, clientResponse) =>
            {
                GCHandle.FromIntPtr(context).Free();
                if (clientResponse.result > OCStackResult.OC_STACK_RESOURCE_CHANGED)
                {
                    tcs.SetException(new Exception("Resource returned error: " + clientResponse.result.ToString()));
                }
                else
                {
                    tcs.SetResult(new ClientResponse<Payload>(clientResponse));
                }
                return OCStackApplicationResult.OC_STACK_DELETE_TRANSACTION;
            };
            var gcHandle = GCHandle.Alloc(handler);
            callbackData.cb = handler;
            callbackData.context = GCHandle.ToIntPtr(gcHandle);
            
            IntPtr payloadHandle = IntPtr.Zero;
            if (resourceTypeName != null)
            {
                RepPayload payload = new RepPayload(data);

                payload.SetUri(_resourceUri);
                payload.Types.Add(resourceTypeName);
                payloadHandle = payload.AsOCRepPayload();
            }

            var result = OCStack.OCDoResource(out _handle, method, _resourceUri, _address.OCDevAddr, payloadHandle, OCConnectivityType.CT_DEFAULT, OCQualityOfService.OC_LOW_QOS, callbackData, null, 0);
            if (payloadHandle != IntPtr.Zero)
                OCPayloadInterop.OCPayloadDestroy(payloadHandle);
            OCStackException.ThrowIfError(result, "Failed to send to resource");
            var response = await tcs.Task.ConfigureAwait(false);
            
            return response;
        }

        public void Dispose()
        {
            OCStack.OCDeleteResource(_handle);
        }

        private OCStackApplicationResult OnObserveCallback(IntPtr context, IntPtr handle, OCClientResponse clientResponse)
        {
            var payload = clientResponse.payload == IntPtr.Zero ? null : new RepPayload(clientResponse.payload);
            OnObserve?.Invoke(this, new ResourceObservationEventArgs(new DeviceAddress(clientResponse.devAddr), clientResponse.resourceUri, payload));
            return OCStackApplicationResult.OC_STACK_KEEP_TRANSACTION;
        }

        public event EventHandler<ResourceObservationEventArgs> OnObserve;
    }

    public class ResourceObservationEventArgs : EventArgs
    {
        internal ResourceObservationEventArgs(DeviceAddress deviceAddress, string resourceUri, RepPayload payload)
        {
            DeviceAddress = deviceAddress;
            ResourceUri = resourceUri;
            Payload = payload;
        }

        public DeviceAddress DeviceAddress { get; }
        public RepPayload Payload { get; }
        public string ResourceUri { get; }
    }
}
