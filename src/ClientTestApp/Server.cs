using System;
using System.Collections.Generic;
using IotivityDotNet.Interop;
using IotivityDotNet;

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
            light.Dispose();
            bswitch.Dispose();
        }

        static LightDevice light;
        static BinarySwitchDevice bswitch;

        private static void CreateResources()
        {
            bswitch = new BinarySwitchDevice("/switch/1");
            light = new LightDevice("/light/1");
        }
    }
    internal static class OicDeviceTypeConstants
    {
        internal const string Bridge = "oic.d.bridge";
        internal const string Switch = "oic.d.switch";
        internal const string Light = "oic.d.light";
        internal const string Fan = "oic.d.fan";
    }
    internal static class OicResourceTypeConstants
    {
        internal const string SwitchBinary = "oic.r.switch.binary";
        internal const string SwitchTemperature = "oic.r.switch.temperature";
        internal const string OperationalState = "oic.r.operational.state";
        internal const string LightBrightness = "oic.r.light.brightness";
        internal const string ColourChroma = "oic.r.colour.chroma"; //No I didn't come up with this stupid spelling ;-)
    }
    public class BinarySwitchDevice : DeviceResource
    {
        
        public BinarySwitchDevice(string uri) : base(uri, OicResourceTypeConstants.SwitchBinary, new Dictionary<string, object> { ["state"] = false })
        {
            BindInterface(OicDeviceTypeConstants.Switch);
        }
        protected override OCEntityHandlerResult OnPropertyUpdated(RepPayload payload)
        {
            bool state;
            if (payload.TryGetBool("state", out state))
            {
                SetProperty(OicResourceTypeConstants.SwitchBinary, "state", state);
                return OCEntityHandlerResult.OC_EH_OK;
            }
            return OCEntityHandlerResult.OC_EH_NOT_ACCEPTABLE;
        }
    }

    public class LightDevice : BinarySwitchDevice
    {
        public LightDevice(string uri, bool isDimmable = true) : base(uri)
        {
            BindInterface(OicDeviceTypeConstants.Light);
            if (isDimmable)
                AddResourceType(OicResourceTypeConstants.LightBrightness, new Dictionary<string, object> { ["brightness"] = 100L });
            AddResourceType(OicResourceTypeConstants.ColourChroma, new Dictionary<string, object> { ["hue"] = 0L, ["saturation"] = 0L, ["csc"] = new double[] { 0d, 0d }, ["ct"] = 0L });
        }

        protected override OCEntityHandlerResult OnPropertyUpdated(RepPayload payload)
        {
            long brightness;
            if (payload.TryGetInt64("brightness", out brightness))
            {
                if (brightness < 0 || brightness > 100)
                    return OCEntityHandlerResult.OC_EH_NOT_ACCEPTABLE;

                SetProperty(OicResourceTypeConstants.LightBrightness, "brightness", brightness);

                return OCEntityHandlerResult.OC_EH_OK;
            }
            return base.OnPropertyUpdated(payload);
        }
    }
}
