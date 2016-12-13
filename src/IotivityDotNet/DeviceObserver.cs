using IotivityDotNet.Interop;
using IotivityNet.OC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class ResourceObserver
    {
        IntPtr _handle;
        OCCallbackData _callbackData;
        public ResourceObserver(DeviceAddress address, string resourceUri)
        {
            _callbackData = new OCCallbackData();
            _callbackData.cb = OnObserveCallback;
            var result = OCStack.OCDoResource(out _handle, OCMethod.OC_REST_OBSERVE, resourceUri, address.OCDevAddr, IntPtr.Zero, OCConnectivityType.CT_DEFAULT, OCQualityOfService.OC_LOW_QOS, _callbackData, null, 0);
            if(result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception("Failed to observe resource: " + result.ToString());
            }
        }
        ~ResourceObserver()
        { 
            //TODO: Destroy _handle
        }

        private OCStackApplicationResult OnObserveCallback(IntPtr context, IntPtr handle, OCClientResponse clientResponse)
        {
            Console.WriteLine("OnObserve {0}:{1} : {2}", clientResponse.devAddr.addr, clientResponse.devAddr.port, clientResponse.result);
            //var ocpayload = Marshal.PtrToStructure(clientResponse.payload, typeof(OCPayload)) as OCPayload;
            var payload = new IotivityNet.OC.RepPayload(clientResponse.payload);
            OnObserve?.Invoke(this, payload);
            // bool state = false;
            // if (payload.TryGetBool("state", out state))
            // {
            //     Console.WriteLine($"Light state: {state}");
            // }
            // IotivityNet.OC.RepPayload reppayload = new IotivityNet.OC.RepPayload();
            // reppayload.SetProperty("state", !state);

            //  var c2 = new OCCallbackData() { cb = OnPost };
            //  OCStack.OCDoResource(out cHandle, OCMethod.OC_REST_POST, gUri, gDestination, reppayload.Handle, OCConnectivityType.CT_DEFAULT, OCQualityOfService.OC_LOW_QOS, c2, null, 0);

            return OCStackApplicationResult.OC_STACK_KEEP_TRANSACTION;
        }
        public event EventHandler<RepPayload> OnObserve;
    }
}
