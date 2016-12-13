using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class DeviceResource : IDisposable
    {
        private readonly IntPtr _handle;
        private readonly string _uri;
        private readonly string _resourceTypeName;
        private readonly string _resourceInterfaceName;

        public DeviceResource(string resourceTypeName, string resourceInterfaceName, string uri)
        {
            _uri = uri;
            _resourceTypeName = resourceTypeName;
            _resourceInterfaceName = resourceInterfaceName;

            OCStackResult result = OCStack.OCCreateResource(out _handle, resourceTypeName, resourceInterfaceName, uri, OCEntityHandler, IntPtr.Zero, OCResourceProperty.OC_DISCOVERABLE | OCResourceProperty.OC_OBSERVABLE);
            if(result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception("Failed to create resource: " + result.ToString());
            }
        }

        public void Dispose()
        {
            OCStack.OCDeleteResource(_handle);
        }

        private OCEntityHandlerResult OCEntityHandler(OCEntityHandlerFlag flag, OCEntityHandlerRequest entityHandlerRequest, IntPtr callbackParam)
        {
            OCEntityHandlerResult result = OCEntityHandlerResult.OC_EH_OK;
            IotivityNet.OC.Payload payload = null;
            if (entityHandlerRequest != null && (flag.HasFlag(OCEntityHandlerFlag.OC_REQUEST_FLAG)))
            {
                switch (entityHandlerRequest.method)
                {
                    case OCMethod.OC_REST_GET:
                        {
                            var rpayload = new IotivityNet.OC.RepPayload();
                            rpayload.SetUri(_uri);
                            rpayload.PopulateFromDictionary(Properties);
                            rpayload.AddResourceType(_resourceTypeName);
                            payload = rpayload;
                        }
                        break;
                    case OCMethod.OC_REST_POST:
                    case OCMethod.OC_REST_PUT:
                        {
                            //var p = entityHandlerRequest.payload;
                            OnPropertyUpdated(this, new IotivityNet.OC.RepPayload(entityHandlerRequest.payload));
                        }
                        break;
                    default:
                        result = OCEntityHandlerResult.OC_EH_METHOD_NOT_ALLOWED;
                        break;
                }
                var response = new OCEntityHandlerResponse();
                response.requestHandle = entityHandlerRequest.requestHandle;
                response.resourceHandle = entityHandlerRequest.resource;
                response.ehResult = result;
                response.payload = payload == null ? IntPtr.Zero : payload.Handle;
                response.numSendVendorSpecificHeaderOptions = 0;
                response.sendVendorSpecificHeaderOptions = IntPtr.Zero;
                response.resourceUri = string.Empty;
                response.persistentBufferFlag = 0;
                OCStack.OCDoResponse(response);
            }
            return result;
        }

        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public void NotifyPropertyChanged(string name)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<IotivityNet.OC.RepPayload> OnPropertyUpdated;
    }
}
 