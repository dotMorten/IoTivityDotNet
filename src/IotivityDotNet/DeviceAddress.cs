using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace IotivityDotNet
{
    public class DeviceAddress
    {
        internal OCDevAddr OCDevAddr { get; }
        
        internal DeviceAddress(OCDevAddr addr)
        {
            OCDevAddr = addr;
        }
        public string Address => OCDevAddr.addr;

        public ushort Port => OCDevAddr.port;

        public override string ToString() => $"{OCDevAddr.addr}:{OCDevAddr.port}";
    }
}
