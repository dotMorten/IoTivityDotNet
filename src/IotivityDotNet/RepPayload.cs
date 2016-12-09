using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityNet.OC
{
    public class RepPayload  : Payload
    {
        internal RepPayload(IntPtr handle) : base(handle)
        {
        }
        internal RepPayload(GCHandle handle) : base(handle)
        {
        }
        public RepPayload() : this(OCPayloadInterop.OCRepPayloadCreate())
        { 
        }
        public RepPayload Clone()
        {
            return new RepPayload(OCPayloadInterop.OCRepPayloadClone(Handle));
        }
        public bool SetProperty(string name, double value)
        {
            return OCPayloadInterop.OCRepPayloadSetPropDouble(Handle, name, value);
        }
        public bool TryGetDouble(string name, out double value)
        {
            return OCPayloadInterop.OCRepPayloadGetPropDouble(Handle, name, out value);
        }
        public bool SetProperty(string name, bool value)
        {
            return OCPayloadInterop.OCRepPayloadSetPropBool(Handle, name, value);
        }
        public bool TryGetBool(string name, out bool value)
        {
            return OCPayloadInterop.OCRepPayloadGetPropBool(Handle, name, out value);
        }


    }
}
