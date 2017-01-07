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
        private readonly Dictionary<string, IotivityValueDictionary> _resourceProperties;
        private readonly OCEntityHandler _resourceCallback; // Pins the delegate into memory

        ~DeviceResource()
        {
            OCStack.OCDeleteResource(_handle);
        }

        public DeviceResource(string uri, string resourceTypeName, IDictionary<string,object> properties, string resourceInterfaceName = IotivityDotNet.Interop.Defines.OC_RSRVD_INTERFACE_DEFAULT)
        {
            _resourceCallback = this.OCEntityHandler;
            OCStackResult result = OCStack.OCCreateResource(out _handle, resourceTypeName, resourceInterfaceName, uri, _resourceCallback, IntPtr.Zero, OCResourceProperty.OC_DISCOVERABLE | OCResourceProperty.OC_OBSERVABLE);
            OCStackException.ThrowIfError(result, "Failed to create resource");
            _uri = uri;
            _resourceProperties = new Dictionary<string, IotivityValueDictionary>();
            if(properties != null)
                _resourceProperties.Add(resourceTypeName, new IotivityValueDictionary(properties));
        }

        protected void BindInterface(string resourceInterfaceName)
        {
            OCStackResult result = OCStack.OCBindResourceInterfaceToResource(_handle, resourceInterfaceName);
            OCStackException.ThrowIfError(result, "Failed to bind interface");
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
            OCStackException.ThrowIfError(result, "Failed to add resource type");
            _resourceProperties.Add(resourceTypeName, new IotivityValueDictionary(properties));
        }

        public void Dispose()
        {
            OCStack.OCDeleteResource(_handle);
        }

        private OCEntityHandlerResult OCEntityHandler(OCEntityHandlerFlag flag, OCEntityHandlerRequest entityHandlerRequest, IntPtr callbackParam)
        {
            OCEntityHandlerResult result = OCEntityHandlerResult.OC_EH_OK;
            RepPayload payload = null;
            RepPayload requestPayload = entityHandlerRequest.payload == IntPtr.Zero ? null : new RepPayload(entityHandlerRequest.payload);
            if (entityHandlerRequest != null && (flag.HasFlag(OCEntityHandlerFlag.OC_REQUEST_FLAG)))
            {
                switch (entityHandlerRequest.method)
                {
                    case OCMethod.OC_REST_GET:
                        {
                            var rpayload = payload = new RepPayload();
                            rpayload.SetUri(_uri);
                            foreach (var resource in _resourceProperties)
                            {
                                if (requestPayload != null && !requestPayload.Types.Contains(resource.Key))
                                    continue;
                                var repayload = new RepPayload(resource.Value);
                                repayload.Types.Add(resource.Key);
                                rpayload.Next = repayload;
                                rpayload = repayload;
                            }
                            payload = rpayload;
                        }
                        break;
                    case OCMethod.OC_REST_POST:
                    case OCMethod.OC_REST_PUT:
                        {
                            var p = new RepPayload(entityHandlerRequest.payload);
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
                response.payload = payload == null ? IntPtr.Zero : payload.AsOCRepPayload();
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

        protected virtual OCEntityHandlerResult OnPropertyUpdated(RepPayload payload)
        {
            return OCEntityHandlerResult.OC_EH_OK;
        }

        public event EventHandler<RepPayload> PropertyUpdated;
    }
}
 