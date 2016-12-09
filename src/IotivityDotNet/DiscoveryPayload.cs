using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityNet.OC
{
    public class ResourcePayload
    {
        private OCResourcePayload _resource;
        internal ResourcePayload(OCResourcePayload resource)
        {
            _resource = resource;
        }
        public IEnumerable<string> Types
        {
            get
            {
                var ptr = _resource.types;
                while (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure(ptr, typeof(OCStringLL)) as OCStringLL;
                    yield return resource.value;
                    ptr = resource.next;
                }
            }
        }
        public IEnumerable<string> Interfaces
        {
            get
            {
                var ptr = _resource.interfaces;
                while (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure(ptr, typeof(OCStringLL)) as OCStringLL;
                    yield return resource.value;
                    ptr = resource.next;
                }
            }
        }
        
        public string Uri => _resource.uri;
        public ushort Port => _resource.port;
        public bool Secure => _resource.secure;
    }


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
            var resource = Marshal.PtrToStructure(ptr, typeof(OCResourcePayload)) as OCResourcePayload;
            return new ResourcePayload(resource);
        }

        
    }
}
