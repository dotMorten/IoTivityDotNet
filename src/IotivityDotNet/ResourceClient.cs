using IotivityDotNet.Interop;
using IotivityNet.OC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class ResourceClient : IDisposable
    {
        private IntPtr _handle;
        private OCCallbackData _observeCallbackData;
        private string _resourceUri;
        private DeviceAddress _address;

        public ResourceClient(DeviceAddress address, string resourceUri)
        {
            _observeCallbackData = new OCCallbackData();
            _observeCallbackData.cb = OnObserveCallback;
            _resourceUri = resourceUri;
            _address = address;
            var result = OCStack.OCDoResource(out _handle, OCMethod.OC_REST_OBSERVE, resourceUri, address.OCDevAddr, IntPtr.Zero, OCConnectivityType.CT_DEFAULT, OCQualityOfService.OC_LOW_QOS, _observeCallbackData, null, 0);
            if (result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception("Failed to observe resource: " + result.ToString());
            }
        }

        public Task<ClientResponse<RepPayload>> PostAsync(Dictionary<string, object> data)
        {
            return SendAsync(data, OCMethod.OC_REST_POST);
        }

        public Task<ClientResponse<RepPayload>> PutAsync(Dictionary<string, object> data)
        {
            return SendAsync(data, OCMethod.OC_REST_PUT);
        }

        public Task<ClientResponse<RepPayload>> GetAsync()
        {
            return SendAsync(null, OCMethod.OC_REST_GET);
        }

        private async Task<ClientResponse<RepPayload>> SendAsync(Dictionary<string, object> data, OCMethod method)
        {
            var tcs = new TaskCompletionSource<ClientResponse<RepPayload>>();
            var callbackData = new OCCallbackData();
            callbackData.cb = (context, handle, clientResponse) =>
            {
                if (clientResponse.result >OCStackResult.OC_STACK_RESOURCE_CHANGED)
                {
                    tcs.SetException(new Exception("Resource returned error: " + clientResponse.result.ToString()));
                }
                else
                {
                    tcs.SetResult(new ClientResponse<RepPayload>(clientResponse));
                }
                return OCStackApplicationResult.OC_STACK_DELETE_TRANSACTION;
            };
            IntPtr payloadHandle = IntPtr.Zero;
            if (data != null)
            {
                RepPayload payload = new RepPayload();

                //payload.SetUri(_resourceUri);
                payload.PopulateFromDictionary(data);
                //payload.AddResourceType(_resourceTypeName);
                payloadHandle = payload.Handle;
            }

            var result = OCStack.OCDoResource(out _handle, method, _resourceUri, _address.OCDevAddr, payloadHandle, OCConnectivityType.CT_DEFAULT, OCQualityOfService.OC_LOW_QOS, callbackData, null, 0);
            if (result != OCStackResult.OC_STACK_OK)
            {
                tcs.TrySetException(new Exception("Failed to send to resource: " + result.ToString()));
            }
            var response = await tcs.Task.ConfigureAwait(false);
            GC.KeepAlive(callbackData);
            return response;
        }

        public void Dispose()
        {
            OCStack.OCDeleteResource(_handle);
        }

        private OCStackApplicationResult OnObserveCallback(IntPtr context, IntPtr handle, OCClientResponse clientResponse)
        {
            var payload = new IotivityNet.OC.RepPayload(clientResponse.payload);
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
