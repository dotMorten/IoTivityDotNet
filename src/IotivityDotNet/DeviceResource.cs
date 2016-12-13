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
            var payload = new IotivityNet.OC.RepPayload();
            if (entityHandlerRequest != null && (flag.HasFlag(OCEntityHandlerFlag.OC_REQUEST_FLAG)))
            {
                if (entityHandlerRequest.method == OCMethod.OC_REST_GET)
                {
                    payload.SetUri(_uri);
                    foreach (var property in Properties)
                    {
                        if (property.Value == null)
                        {
                            payload.SetPropertyNull(property.Key);
                        }
                        else if (property.Value.GetType() == typeof(bool))
                        {
                            payload.SetProperty(property.Key, (bool)property.Value);
                        }
                        else if (property.Value.GetType() == typeof(double))
                        {
                            payload.SetProperty(property.Key, (double)property.Value);
                        }
                        else if (property.Value.GetType() == typeof(long))
                        {
                            payload.SetProperty(property.Key, (long)property.Value);
                        }
                        else if (property.Value.GetType() == typeof(string))
                        {
                            payload.SetProperty(property.Key, (string)property.Value);
                        }
                        else throw new NotSupportedException("Property Type for key '" + property.Key + "' of type " + property.Value.GetType().FullName + " not supported");
                    }
                    payload.AddResourceType(_resourceTypeName);
                    var response = new OCEntityHandlerResponse();
                    response.requestHandle = entityHandlerRequest.requestHandle;
                    response.resourceHandle = entityHandlerRequest.resource;
                    response.ehResult = result;
                    response.payload = payload.Handle;// ocpayload;
                    response.numSendVendorSpecificHeaderOptions = 0;
                    response.sendVendorSpecificHeaderOptions = IntPtr.Zero;
                    response.resourceUri = string.Empty;
                    response.persistentBufferFlag = 0;
                    IotivityDotNet.Interop.OCStack.OCDoResponse(response);
                }
                else if (entityHandlerRequest.method == OCMethod.OC_REST_POST)
                {
                    //TODO
                    result = OCEntityHandlerResult.OC_EH_METHOD_NOT_ALLOWED;
                }
                else if (entityHandlerRequest.method == OCMethod.OC_REST_PUT)
                {
                    result = OCEntityHandlerResult.OC_EH_METHOD_NOT_ALLOWED;
                }
                else
                {
                    result = OCEntityHandlerResult.OC_EH_METHOD_NOT_ALLOWED;
                }
            }
            return result;
        }

        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public void NotifyPropertyChanged(string name)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<Dictionary<string, object>> OnPropertyUpdated;
    }
}
 