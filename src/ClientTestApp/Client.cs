using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestApp
{
    public class Client : IDisposable
    {
        private IotivityNet.OC.DiscoverResource svc;
        public Client()
        {
            //Search for services
            svc = new IotivityNet.OC.DiscoverResource("/oic/res");
            svc.ResourceDiscovered += Svc_ResourceDiscovered;
            svc.Start();
        }

        public void Dispose()
        {
            svc.Stop();
        }

        private void Svc_ResourceDiscovered(object sender, IotivityNet.OC.ClientResponseEventArgs<IotivityNet.OC.DiscoveryPayload> e)
        {
            Console.WriteLine($"Device Discovered @ {e.Response.DeviceAddress}");
            foreach (var r in e.Response.Payload.Resources)
            {
                //Print out resources, their interfaces and types on the device
                var uri = r.Uri;
                if (!string.IsNullOrWhiteSpace(r.Uri) && !r.Uri.StartsWith("/oic/")) //Skips the generic oic ones
                {
                    Console.WriteLine("\t" + r.Uri + (r.Secure ? " (secure)" : ""));
                    Console.WriteLine("\t\tInterfaces:");
                    foreach (var iface in r.Interfaces)
                    {
                        Console.WriteLine("\t\t\t" + iface);
                    }
                    Console.WriteLine("\t\tTypes:");
                    foreach (var type in r.Types)
                    {
                        Console.WriteLine("\t\t\t" + type);
                    }
                    if (r.Uri == "/BinarySwitchResURI" || r.Uri == "/light/1")
                    {
                        var observer = new IotivityDotNet.ResourceObserver(e.Response.DeviceAddress, r.Uri);
                        observer.OnObserve += Observer_OnObserve;
                    }
                }
            }
        }


        private void Observer_OnObserve(object sender, IotivityNet.OC.RepPayload e)
        {
            bool state;
            if (e.TryGetBool("state", out state))
            {
                Console.WriteLine($"State of device: {state}");
            }
        }
    }
}
