﻿using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class ResourcePayload : Payload
    {
        private OCResourcePayload _resource;

        internal ResourcePayload(IntPtr ptr) : base(ptr)
        {
            _resource = Marshal.PtrToStructure<OCResourcePayload>(ptr);
        }

        public IEnumerable<string> Types
        {
            get
            {
                var ptr = _resource.types;
                if (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure<OCStringLL>(ptr);
                    return resource.Values;
                }
                return new string[] { };
            }
        }

        public IEnumerable<string> Interfaces
        {
            get
            {
                var ptr = _resource.interfaces;
                if (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure<OCStringLL>(ptr);
                    return resource.Values;
                }
                return new string[] { };
            }
        }

        public string Uri => _resource.uri;

        public ushort Port => _resource.port;

        public bool Secure => _resource.secure;
    }
}
