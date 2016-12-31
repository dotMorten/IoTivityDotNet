using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class DiscoveryPayload : Payload
    {
        public DiscoveryPayload() : this(OCPayloadInterop.OCDiscoveryPayloadCreate()) { }

        internal DiscoveryPayload(IntPtr handle) : base(handle)
        {
        }

        internal DiscoveryPayload(GCHandle handle) : base(handle)
        {
        }

        public static DiscoveryPayload Create()
        {
            return new DiscoveryPayload();
        }
      
        public ulong ResourceCount { get { return (ulong)OCPayloadInterop.OCDiscoveryPayloadGetResourceCount(Handle); } }

        public IEnumerable<ResourcePayload> Resources
        {
            get
            {
                for (ulong i = 0; i < ResourceCount; i++)
                {
                    yield return GetResource(i);
                }
            }
        }

        public ResourcePayload GetResource(ulong index)
        {
            var ptr = OCPayloadInterop.OCDiscoveryPayloadGetResource(Handle, (UIntPtr)index);
            if (ptr == IntPtr.Zero)
                return null;
            return new ResourcePayload(ptr);
        }
    }
}
