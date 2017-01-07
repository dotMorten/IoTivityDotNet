using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace IotivityDotNet
{
    public class RepPayload  : Payload
    {
        public RepPayload() : base()
        {
            Interfaces = new List<string>();
            Types = new List<string>();
            Values = new IotivityValueDictionary();
        }

        internal RepPayload(IntPtr handle) : this()
        {
            var ocpayload = Marshal.PtrToStructure<OCRepPayload>(handle);
            if (ocpayload.interfaces != IntPtr.Zero)
            {
                var resource = Marshal.PtrToStructure<OCStringLL>(ocpayload.interfaces);
                Interfaces = resource.Values.ToArray();
            }
            if (ocpayload.types != IntPtr.Zero)
            {
                var resource = Marshal.PtrToStructure<OCStringLL>(ocpayload.types);
                Types = resource.Values.ToArray();
            }
            if (ocpayload.next != IntPtr.Zero)
                Next = new RepPayload(ocpayload.next);
            Values = new IotivityValueDictionary(ocpayload);
        }
        

        public IList<string> Types { get; }

        public IList<string> Interfaces { get; }
        
        public RepPayload(IDictionary<string, object> data = null) : this()
        {
            if (data is IotivityValueDictionary)
                Values = data;
            else
                Values = new IotivityValueDictionary(data);
        }

        public RepPayload Next { get; set; }

        public IDictionary<string,object> Values { get; }

        public void AddValues(IEnumerable<KeyValuePair<string,object>> data)
        {
            foreach (var property in data)
            {
                Values.Add(property);
            }
        }

        private string _uri;

        public void SetUri(string uri)
        {
            _uri = uri;
        }


        public IntPtr AsOCRepPayload()
        {
            IntPtr handle = OCPayloadInterop.OCRepPayloadCreate();
            bool ok = false;
            if(!string.IsNullOrEmpty(_uri))
                ok = OCPayloadInterop.OCRepPayloadSetUri(handle, _uri);
            (Values as IotivityValueDictionary).AssignToOCRepPayload(handle);
            foreach (var resourceType in Types)
                ok = OCPayloadInterop.OCRepPayloadAddResourceType(handle, resourceType);
            if(Next != null)
            {
                OCPayloadInterop.OCRepPayloadAppend(handle, Next.AsOCRepPayload());
            }
            return handle;
        }

        public bool TryGetBool(string key, out bool value)
        {
            return TryGet<bool>(key, out value);
        }

        public bool TryGetInt64(string key, out long value)
        {
            return TryGet<long>(key, out value);
        }

        public bool TryGetDouble(string key, out double value)
        {
            return TryGet<double>(key, out value);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            if (!Values.ContainsKey(key))
                return false;
            var v = Values[key];
            if (v is T)
            {
                value = (T)v;
                return true;
            }
            return false;
        }
    }
}
