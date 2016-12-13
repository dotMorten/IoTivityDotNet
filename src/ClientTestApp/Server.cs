using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestApp
{
    public class Server : IDisposable
    {
        public Server()
        {
            CreateResources();
        }

        public void Dispose()
        {
            _switchResource.Dispose();
            _lightResource.Dispose();
        }

        private static IotivityDotNet.DeviceResource _switchResource;
        private static IotivityDotNet.DeviceResource _lightResource;

        private static void CreateResources()
        {
            // Create switch
            _switchResource = new IotivityDotNet.DeviceResource("oic.r.switch.binary", "oic.if.baseline", "/BinarySwitchResURI");
            _switchResource.Properties["name"] = "Mock Switch";
            _switchResource.Properties["state"] = false;
            _switchResource.OnPropertyUpdated += Resource_OnPropertyUpdated;

            // Create light
            _lightResource = new IotivityDotNet.DeviceResource("core.light", "oic.if.baseline", "/light/1");
            _lightResource.Properties["name"] = "Mock Light";
            _lightResource.Properties["state"] = true;
            _lightResource.Properties["hue"] = 90d;
            _lightResource.Properties["brightness"] = .5;
            _lightResource.Properties["saturation"] = 1d;
            _lightResource.OnPropertyUpdated += Resource_OnPropertyUpdated;
        }

        private static void Resource_OnPropertyUpdated(object sender, IotivityNet.OC.RepPayload e)
        {
            bool state;
            if(e.TryGetBool("state", out state))
            {
                (sender as IotivityDotNet.DeviceResource).Properties["state"] = state;
                Console.WriteLine("State updated to :" + state);
            }
        }
    }
}
