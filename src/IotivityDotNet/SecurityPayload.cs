using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class SecurityPayload : Payload
    {
        internal SecurityPayload(IntPtr ptr) 
        {
            var _resource = Marshal.PtrToStructure<OCSecurityPayload>(ptr);
            SecurityData = _resource.SecurityData;
        }

        public SecurityPayload(byte[] securityData)
        {
            SecurityData = securityData;
        }

        public byte[] SecurityData { get; }

        internal IntPtr AsOCSecurityPayload()
        {
            return Interop.OCPayloadInterop.OCSecurityPayloadCreate(SecurityData, (UIntPtr)SecurityData.Length);
        }
    }
}
