using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IotivityNet
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
            //var result = OCStack.OCInit1((OCMode)mode, OCTransportFlags.OC_DEFAULT_FLAGS, OCTransportFlags.OC_DEFAULT_FLAGS);
            if(result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception(result.ToString());
            }
            ct = new CancellationTokenSource();
            tcs = new TaskCompletionSource<object>();

#if NETFX_CORE
            Task.Run(async () =>
#else
            ThreadPool.QueueUserWorkItem(async (s) =>
#endif
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
        
        public static async Task Shutdown()
        {
            if (tcs == null) return;
            ct.Cancel();
            await tcs.Task;
            var result = OCStack.OCStop();
            if (result != OCStackResult.OC_STACK_OK)
            {
                throw new Exception(result.ToString());
            }
        }
    }
}
