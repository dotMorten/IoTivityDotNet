using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            IotivityNet.Service.Initialize(IotivityNet.ServiceMode.Client);
            Console.WriteLine("Running OCStack. Press any key to close\n");

            //Search for services
            var svc = new IotivityNet.OC.DiscoverResource("/oic/res");
            svc.ResourceDiscovered += (s, e) =>
            {
                Console.WriteLine($"Device Discovered @ {e.Response.DeviceAddress}");
                foreach (var r in e.Response.Payload.Resources)
                {
                    //Print out resources, their interfaces and types on the device
                    var uri = r.Uri;
                    if (!string.IsNullOrWhiteSpace(r.Uri) && !r.Uri.StartsWith("/oic/")) //Skips the generic oic ones
                    {
                        Console.WriteLine("\t" + r.Uri + (r.Secure ? " (secure)" : ""));
                        foreach (var iface in r.Interfaces)
                        {
                            Console.WriteLine("\t\tI: " + iface);
                        }
                        foreach (var type in r.Types)
                        {
                            Console.WriteLine("\t\tT: " + type);
                        }
                    }
                }
            };
            svc.Start();

            var key = Console.ReadKey();
            Console.WriteLine("Shutting down...");
            svc.Stop();
            IotivityNet.Service.Shutdown().Wait();
        }
    }
}
