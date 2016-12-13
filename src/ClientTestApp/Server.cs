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
            _switchResource.Properties.Add("state", false);

            // Create light
            _lightResource = new IotivityDotNet.DeviceResource("core.light", "oic.if.baseline", "/light/1");
            _lightResource.Properties["state"] = true;
            _lightResource.Properties["hue"] = 90d;
            _lightResource.Properties["brightness"] = .5;
            _lightResource.Properties["saturation"] = 1d;
        }
    }
}
