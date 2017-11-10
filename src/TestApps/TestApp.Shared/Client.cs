using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestApp
{
    public class Client : IDisposable
    {
        private IotivityDotNet.DiscoverResource svc;
        public Client()
        {
            //Search for services
            svc = new IotivityDotNet.DiscoverResource();
            svc.ResourceDiscovered += ResourceDiscovered;
            svc.Start();
        }

        public void Dispose()
        {
            svc.Stop();
        }

        private async void ResourceDiscovered(object sender, IotivityDotNet.ResourceDiscoveredEventArgs e)
        {
            Log.WriteLine($"Device Discovered @ {e.Address}");
            var r = e.Payload;
            {
                //Print out resources, their interfaces and types on the device
                var uri = r.Uri;
                if (!string.IsNullOrWhiteSpace(r.Uri) && !r.Uri.StartsWith("/oic/")) //Skips the generic oic ones
                {
                    Log.WriteLine("\t" + r.Uri + (r.Secure ? " (secure)" : ""));
                    Log.WriteLine("\t\tInterfaces:");
                    foreach (var iface in r.Interfaces)
                    {
                        Log.WriteLine("\t\t\t" + iface);
                    }
                    Log.WriteLine("\t\tTypes:");
                    foreach (var type in r.Types)
                    {
                        Log.WriteLine("\t\t\t" + type);
                        //if (r.Uri == "/BinarySwitchResURI" || r.Uri == "/light/1")
                        {
                            var client = new IotivityDotNet.ResourceClient(e.Address, r.Uri);
                            //Get all the properties from the resource
                            var response = await client.GetAsync(type);
                            var p = response.Payload as IotivityDotNet.RepPayload;
                            while(p != null)
                            {
                                foreach(var item in p.Values)
                                {
                                    Console.WriteLine($"\t\t\t\t{item.Key} = {item.Value}");
                                }
                                p = p.Next;
                            }

                            //Start observing the resource
                            client.OnObserve += OnResourceObserved;
                        }
                    }
                }
            }
        }
        private async void OnResourceObserved(object sender, IotivityDotNet.ResourceObservationEventArgs e)
        {
            var payload = e.Payload;
            Log.WriteLine($"Resource observed @ {e.DeviceAddress} {e.ResourceUri}");
            while (payload != null)
            {
                foreach(var type in payload.Types)
                {
                    Log.WriteLine($"\t{type}");
                }
                foreach (var iface in payload.Interfaces)
                {
                    Log.WriteLine($"\t{iface}");
                }
                foreach (var pair in payload.Values)
                {
                    Log.WriteLine($"\t\t{pair.Key}: {pair.Value}");
                }
                //Update the resource switch state (flip it)
                bool isSwitch = payload.Types.Contains("oic.r.switch.binary");
                bool state = false;
                if (isSwitch && payload.TryGetBool("state", out state))
                {
                    var client = sender as IotivityDotNet.ResourceClient;
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data["state"] = !state;
                    await client.PostAsync("oic.r.switch.binary", data);
                }

                payload = payload.Next;
            }
        }
    }
}
