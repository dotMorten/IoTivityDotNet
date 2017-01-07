using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class ResourcePayload  : Payload
    {
        internal ResourcePayload(IntPtr ptr) : base()
        {
            var _resource = Marshal.PtrToStructure<OCResourcePayload>(ptr);
            Uri = _resource.uri;
            Port = _resource.port;
            Secure = _resource.secure;
            if (_resource.interfaces != IntPtr.Zero)
            {
                var resource = Marshal.PtrToStructure<OCStringLL>(_resource.interfaces);
                Interfaces = resource.Values.ToArray();
            }
            else
                Interfaces = Enumerable.Empty<string>();

            if (_resource.types != IntPtr.Zero)
            {
                var resource = Marshal.PtrToStructure<OCStringLL>(_resource.types);
                Types = resource.Values.ToArray();
            }
            else
                Types = Enumerable.Empty<string>();
        }

        public IEnumerable<string> Types { get; }

        public IEnumerable<string> Interfaces { get; }

        public string Uri { get; }

        public ushort Port { get; }

        public bool Secure { get; }
    }
}
