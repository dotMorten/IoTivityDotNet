using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public enum ServiceMode
    {
        Client = OCMode.OC_CLIENT,
        Server = OCMode.OC_SERVER,
        ClientServer = OCMode.OC_CLIENT_SERVER,
        Gateway = OCMode.OC_GATEWAY          /**< Client server mode along with routing capabilities.*/
    }

    public static class Service
    {
        private static CancellationTokenSource ct;
        private static TaskCompletionSource<object> tcs;

        public static void Initialize(ServiceMode mode)
        {
            var result = OCStack.OCInit(null, 0, (OCMode)mode);
            OCStackException.ThrowIfError(result);
            result = OCStack.OCSetDefaultDeviceEntityHandler(OCDefaultDeviceEntityHandler, IntPtr.Zero);
            OCStackException.ThrowIfError(result, "Failed to send to resource");

            ct = new CancellationTokenSource();
            tcs = new TaskCompletionSource<object>();

            Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    var result2 = OCStack.OCProcess();
                    if (result2 != OCStackResult.OC_STACK_OK)
                    {
                        // tcs.SetException(new Exception("OCStackException on Process: " + result2.ToString()));
                        // break;
                    }
                    await Task.Delay(1);
                }
                tcs.SetResult(true);
                tcs = null;
                ct = null;
            });
        }

        //public static byte[] DeviceId
        //{
        //    get
        //    {
        //        OCUUIdentity identity = null;
        //        var result = OCStack.OCGetDeviceId(out identity);
        //        if (result != OCStackResult.OC_STACK_OK)
        //        {
        //            throw new Exception(result.ToString());
        //        }
        //        return identity.deviceId;
        //    }
        //    set
        //    {
        //        if (value == null || value.Length > 16)
        //        {
        //            throw new ArgumentException(nameof(value));
        //        }
        //        var result = OCStack.OCSetDeviceId(new OCUUIdentity() { deviceId = value });
        //        if (result != OCStackResult.OC_STACK_OK)
        //        {
        //            throw new Exception(result.ToString());
        //        }
        //    }
        //}

        /// <summary>
        /// This function sets device information.
        /// Upon call to OCInit, the default Device Type (i.e. "rt") has already been set to the default
        /// Device Type "oic.wk.d". You do not have to specify "oic.wk.d" in the OCDeviceInfo.types linked
        /// list. The default Device Type is mandatory and always specified by this Device as the first
        /// Device Type.
        /// </summary>
        public static void SetDeviceInfo(string deviceName, IEnumerable<string> types, string specVersion, IEnumerable<string> dataModelVersions)
        {
            OCStringLL octypes = OCStringLL.Create(types);
            OCStringLL ocdataModelVersions = OCStringLL.Create(dataModelVersions);
            var info = new OCDeviceInfo()
            {
                deviceName = deviceName,
                types = octypes,
                dataModelVersions = ocdataModelVersions,
                specVersion = specVersion
            };
            var result = OCStack.OCSetDeviceInfo(info);
            OCStackException.ThrowIfError(result);
        }

        private static OCEntityHandlerResult OCDefaultDeviceEntityHandler(OCEntityHandlerFlag flag, OCEntityHandlerRequest entityHandlerRequest, IntPtr callbackParam)
        {
            return OCEntityHandlerResult.OC_EH_OK;
        }

        public static async Task Shutdown()
        {
            if (tcs == null) return;
            ct.Cancel();
            await tcs.Task;
            var result = OCStack.OCStop();
            OCStackException.ThrowIfError(result);
        }
    }
}
