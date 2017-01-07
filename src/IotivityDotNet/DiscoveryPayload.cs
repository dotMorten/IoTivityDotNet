using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IotivityDotNet
{
    public class DiscoveryPayload  : Payload
    {
        internal DiscoveryPayload(IntPtr handle) : base()
        {
            ulong count = (ulong)OCPayloadInterop.OCDiscoveryPayloadGetResourceCount(handle);
            var resources = new List<ResourcePayload>((int)count);
            for (ulong i = 0; i < count; i++)
            {
                resources.Add(GetResource(handle, i));
            }
            Resources = new ReadOnlyCollection<ResourcePayload>(resources);
        }

        public IReadOnlyList<ResourcePayload> Resources { get; }

        private static ResourcePayload GetResource(IntPtr handle, ulong index)
        {
            var ptr = OCPayloadInterop.OCDiscoveryPayloadGetResource(handle, (UIntPtr)index);
            if (ptr == IntPtr.Zero)
                return null;
            return new ResourcePayload(ptr);
        }
    }
}
