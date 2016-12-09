using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace IotivityNet.OC
{
    public class DeviceAddress
    {
        private OCDevAddr _addr;

        internal DeviceAddress(OCDevAddr addr)
        {
            _addr = addr;
        }
        public string Address => _addr.addr;

        public ushort Port => _addr.port;

        public override string ToString() => $"{_addr.addr}:{_addr.port}";
    }
}
