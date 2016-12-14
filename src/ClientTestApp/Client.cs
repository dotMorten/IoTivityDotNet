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
            svc = new IotivityNet.OC.DiscoverResource();
            svc.ResourceDiscovered += ResourceDiscovered;
            svc.Start();
        }

        public void Dispose()
        {
            svc.Stop();
        }

        private async void ResourceDiscovered(object sender, IotivityNet.OC.ClientResponseEventArgs<IotivityNet.OC.DiscoveryPayload> e)
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
                    //if (r.Uri == "/BinarySwitchResURI" || r.Uri == "/light/1")
                    {
                        var client = new IotivityDotNet.ResourceClient(e.Response.DeviceAddress, r.Uri);
                        //Get all the properties from the resource
                        // var response = await client.GetAsync();
                        // bool state;
                        // if(response.Payload.TryGetBool("state", out state))
                        // {
                        //     Console.WriteLine("The state of the resource is: " + state.ToString());
                        // }

                        //Start observing the resource
                        client.OnObserve += OnResourceObserved;

                      
                    }
                }
            }
        }
        private void OnResourceObserved(object sender, IotivityDotNet.ResourceObservationEventArgs e)
        {
            var payload = e.Payload;
            Console.WriteLine($"Resource observed @ {e.DeviceAddress} {e.ResourceUri}");
            while (payload != null)
            {
                foreach(var type in payload.Types)
                {
                    Console.WriteLine($"\t{type}");
                }
                foreach (var iface in payload.Interfaces)
                {
                    Console.WriteLine($"\t{iface}");
                }
                foreach (var pair in payload.Values)
                {
                    Console.WriteLine($"\t\t{pair.Key}: {pair.Value}");
                }
                payload = payload.Next;
            }

            //Update the resource
            //bool state = false;
            //if (payload.TryGetBool("state", out state))
            //{
            //    var client = sender as IotivityDotNet.ResourceClient;
            //    Dictionary<string, object> data = new Dictionary<string, object>();
            //    data["state"] = !state;
            //    client.PostAsync(data);
            //}
        }
    }
}
