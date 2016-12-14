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
        private IntPtr _handle;
        private readonly string _uri;
        private Dictionary<string, Dictionary<string, object>> _resourceProperties;

        public DeviceResource(string uri, string resourceTypeName, IDictionary<string,object> properties, string resourceInterfaceName = "oic.if.baseline")
        {
            OCStackResult result = OCStack.OCCreateResource(out _handle, resourceTypeName, resourceInterfaceName, uri, OCEntityHandler, IntPtr.Zero, OCResourceProperty.OC_DISCOVERABLE | OCResourceProperty.OC_OBSERVABLE);
            if (result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception("Failed to create resource: " + result.ToString());
            }
            _uri = uri;
            _resourceProperties = new Dictionary<string, Dictionary<string, object>>();
            _resourceProperties.Add(resourceTypeName, new Dictionary<string, object>(properties));
        }

        protected void BindInterface(string resourceInterfaceName)
        {
            OCStackResult result = OCStack.OCBindResourceInterfaceToResource(_handle, resourceInterfaceName);
            if (result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception("Failed to bind interface name: " + result.ToString());
            }
        }

        public void SetProperty(string resourceTypeName, string property, object value)
        {
            _resourceProperties[resourceTypeName][property] = value;
        }
        public object GetProperty(string resourceTypeName, string property)
        {
            return _resourceProperties[resourceTypeName][property];
        }
        public IEnumerable<string> ResourceTypes => _resourceProperties.Keys;

        public void AddResourceType(string resourceTypeName, IDictionary<string, object> properties)
        {
            OCStackResult result = OCStack.OCBindResourceTypeToResource(_handle, resourceTypeName);
            if (result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception("Failed to create resource: " + result.ToString());
            }
            _resourceProperties.Add(resourceTypeName, new Dictionary<string, object>(properties));
        }

        public void Dispose()
        {
            OCStack.OCDeleteResource(_handle);
            _resourceProperties = null;
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
                            foreach (var resource in _resourceProperties)
                            {
                                var repayload = new IotivityNet.OC.RepPayload(resource.Value);
                                repayload.AddResourceType(resource.Key);
                                rpayload.Append(repayload);
                            }
                            payload = rpayload;
                        }
                        break;
                    case OCMethod.OC_REST_POST:
                    case OCMethod.OC_REST_PUT:
                        {
                            var p = new IotivityNet.OC.RepPayload(entityHandlerRequest.payload);
                            result = OnPropertyUpdated(p);
                            PropertyUpdated?.Invoke(this, p);
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

        //public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public void NotifyPropertyChanged(string name)
        {
            throw new NotImplementedException();
        }

        protected virtual OCEntityHandlerResult OnPropertyUpdated(IotivityNet.OC.RepPayload payload)
        {
            return OCEntityHandlerResult.OC_EH_OK;
        }

        public event EventHandler<IotivityNet.OC.RepPayload> PropertyUpdated;
    }
}
 