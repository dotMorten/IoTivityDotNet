using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityNet.OC
{
    public abstract class Payload
    {
        GCHandle gchandle;
        internal static Payload FromOCPayload(OCPayload payload)
        {
            switch (payload.type)
            {
                case OCPayloadType.PAYLOAD_TYPE_DISCOVERY:
                    return new DiscoveryPayload(GCHandle.Alloc(payload));
                case OCPayloadType.PAYLOAD_TYPE_REPRESENTATION:
                    return new RepPayload(GCHandle.Alloc(payload));
            }
            return null;
        }

        protected Payload(IntPtr handle)
        {
            Handle = handle;
        }
        protected Payload(GCHandle gchandle) : this(GCHandle.ToIntPtr(gchandle))
        {
            this.gchandle = gchandle;
        }

        internal IntPtr Handle { get; }

        ~Payload()
        {
           //  if (gchandle.IsAllocated)
           //      gchandle.Free();
           //  OCPayloadDestroy(Handle);
        }
    }

}
