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
        static LightDevice light2;
        static LightDevice light3;
        static BinarySwitchDevice bswitch;
        static BinarySwitchDevice bswitch2;
        static BinarySwitchDevice bswitch3;
        static BinarySwitchDevice bswitch4;

        private static void CreateResources()
        {
            //bswitch = new BinarySwitchDevice("/switch/1", false);
            //bswitch2 = new BinarySwitchDevice("/switch/2", true);
            // bswitch3 = new BinarySwitchDevice("/switch/3", true);
            // bswitch4 = new BinarySwitchDevice("/switch/4", true);
            // light = new LightDevice("/light/1", true, 50, 180, 100);
            // light2 = new LightDevice("/foo/1", false, 75);
            // light3 = new LightDevice("/lifx/2", true);
            IntPtr _handle;
            string resourceTypeName = "oic.r.switch.binary";
            string resourceInterfaceName = "oic.if.baseline";

            OCStackResult result = OCStack.OCCreateResource(out _handle, resourceTypeName, resourceInterfaceName,
                "/switch/1", null, IntPtr.Zero, OCResourceProperty.OC_DISCOVERABLE | OCResourceProperty.OC_OBSERVABLE);

            result = OCStack.OCCreateResource(out _handle, resourceTypeName + "2", resourceInterfaceName, 
                "/switch/2", null, IntPtr.Zero, OCResourceProperty.OC_DISCOVERABLE | OCResourceProperty.OC_OBSERVABLE);

            result = OCStack.OCCreateResource(out _handle, resourceTypeName + "3", resourceInterfaceName,
                "/switch/3", null, IntPtr.Zero, OCResourceProperty.OC_DISCOVERABLE | OCResourceProperty.OC_OBSERVABLE);
            
            //result = OCStack.OCCreateResource(out _handle, resourceTypeName + "4", resourceInterfaceName,
            //    "/switch/4", null, IntPtr.Zero, OCResourceProperty.OC_DISCOVERABLE | OCResourceProperty.OC_OBSERVABLE);
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
        internal const string SwitchBinary = "oic.r.switch.binary";           // https://github.com/OpenInterConnect/IoTDataModels/blob/master/binarySwitch.raml
        internal const string SwitchTemperature = "oic.r.temperature";        // https://github.com/OpenInterConnect/IoTDataModels/blob/master/temperature.raml
        internal const string OperationalState = "oic.r.operational.state";   // https://github.com/OpenInterConnect/IoTDataModels/blob/master/operationalState.raml
        internal const string LightBrightness = "oic.r.light.brightness";     // https://github.com/OpenInterConnect/IoTDataModels/blob/master/oic.r.light.brightness.json
        internal const string ColourChroma = "oic.r.colour.chroma";           // https://github.com/OpenInterConnect/IoTDataModels/blob/master/colourChroma.raml
    }

    public class BinarySwitchDevice : DeviceResource
    {        
        public BinarySwitchDevice(string uri, bool isOn) : base(uri, OicResourceTypeConstants.SwitchBinary, new Dictionary<string, object> { ["state"] = isOn ,["name"]="Switch" })
        {
            //AddResourceType(OicResourceTypeConstants.SwitchBinary, new Dictionary<string, object> { ["state"] = isOn, ["name"] = "Switch" });
            BindInterface(OicDeviceTypeConstants.Switch);
        }
        protected override OCEntityHandlerResult OnPropertyUpdated(RepPayload payload)
        {
            bool state;
            if (payload.TryGetBool("state", out state))
            {
                SetProperty(OicResourceTypeConstants.SwitchBinary, "state", state);
                Log.WriteLine($"Switch state updated to '{state}'");
                return OCEntityHandlerResult.OC_EH_OK;
            }
            return OCEntityHandlerResult.OC_EH_NOT_ACCEPTABLE;
        }
    }

    public class LightDevice : BinarySwitchDevice
    {
        public LightDevice(string uri, bool isOn, long? brightness = null, long? hue = null, long? saturation = null) : base(uri, isOn)
        {
            BindInterface(OicDeviceTypeConstants.Light);
            if (brightness.HasValue)
            {
                if (brightness.Value < 0 || brightness.Value > 100) throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 0 and 100");
                AddResourceType(OicResourceTypeConstants.LightBrightness, new Dictionary<string, object> { ["brightness"] = brightness.Value });
            }
            if (hue.HasValue && saturation.HasValue)
            {
                if (hue.Value < 0 || hue.Value > 360) throw new ArgumentOutOfRangeException(nameof(hue), "Hue must be between 0 and 360");
                if (saturation.Value < 0 || saturation.Value > 100) throw new ArgumentOutOfRangeException(nameof(saturation), "Saturation must be between 0 and 100");
                AddResourceType(OicResourceTypeConstants.ColourChroma, new Dictionary<string, object> { ["hue"] = hue.Value, ["saturation"] = saturation.Value, ["csc"] = new double[] { 3d, 7d }, ["ct"] = 0L });
            }
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
