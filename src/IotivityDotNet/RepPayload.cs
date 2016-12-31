using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public class RepPayload  : Payload
    {
        OCRepPayload ocpayload;
        
        internal RepPayload(IntPtr handle) : base(handle)
        {
            ocpayload = Marshal.PtrToStructure<OCRepPayload>(Handle);
        }

        public IEnumerable<string> Types
        {
            get
            {
                var ptr = ocpayload.types;
                if (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure<OCStringLL>(ptr);
                    return resource.Values;
                }
                return Enumerable.Empty<string>();
            }
        }

        public IEnumerable<string> Interfaces
        {
            get
            {
                var ptr = ocpayload.interfaces;
                if (ptr != IntPtr.Zero)
                {
                    var resource = Marshal.PtrToStructure<OCStringLL>(ptr);
                    return resource.Values;
                }
                return Enumerable.Empty<string>();
            }
        }
        internal RepPayload(GCHandle handle) : base(handle)
        {
        }

        public RepPayload(IDictionary<string, object> data = null) : this(OCPayloadInterop.OCRepPayloadCreate())
        {
            if(data != null)
                PopulateFromDictionary(data);
        }

        public RepPayload Next
        {
            get
            {
                var ptr = ocpayload.next;
                if(ptr != IntPtr.Zero)
                {
                    return new RepPayload(ptr);
                }
                return null;
            }
        }
        public IEnumerable<KeyValuePair<string,object>> Values
        {
            get
            {
                var values = ocpayload.values;
                while (values != IntPtr.Zero)
                {
                    var payloadValue = Marshal.PtrToStructure<OCRepPayloadValue>(values);
                    var ptr = payloadValue.value;
                    switch (payloadValue.type)
                    {
                        case OCRepPayloadPropType.OCREP_PROP_NULL:
                            yield return new KeyValuePair<string, object>(payloadValue.name, null); break;
                        case OCRepPayloadPropType.OCREP_PROP_STRING:
                            {
                                var strvalue = Marshal.PtrToStringAnsi(ptr.ocByteStr);
                                yield return new KeyValuePair<string, object>(payloadValue.name, strvalue); break;
                            }
                        case OCRepPayloadPropType.OCREP_PROP_BOOL:
                            {
                                bool bvalue = payloadValue.value.b;
                                yield return new KeyValuePair<string, object>(payloadValue.name, bvalue);
                                break;
                            }
                        case OCRepPayloadPropType.OCREP_PROP_DOUBLE:
                            {
                                double dvalue = payloadValue.value.d;
                                yield return new KeyValuePair<string, object>(payloadValue.name, dvalue);
                                break;
                            }
                        case OCRepPayloadPropType.OCREP_PROP_INT:
                            {
                                long lvalue = payloadValue.value.i;
                                yield return new KeyValuePair<string, object>(payloadValue.name, lvalue);
                                break;
                            }
                        case OCRepPayloadPropType.OCREP_PROP_ARRAY:
                            {
                                IntPtr arr = payloadValue.value.ocByteStr;
                                //var a = Marshal.PtrToStructure<double[]>(arr);
                                break;
                            }
                        default:
                            throw new NotImplementedException(payloadValue.type.ToString());
                    }
                    values = payloadValue.next;
                }
            }
        }
        public void PopulateFromDictionary(IDictionary<string,object> data)
        {
            foreach (var property in data)
            {
                if (property.Value == null)
                {
                    SetPropertyNull(property.Key);
                }
                else if (property.Value.GetType() == typeof(bool))
                {
                    SetProperty(property.Key, (bool)property.Value);
                }
                else if (property.Value.GetType() == typeof(double))
                {
                    SetProperty(property.Key, (double)property.Value);
                }
                else if (property.Value.GetType() == typeof(long))
                {
                    SetProperty(property.Key, (long)property.Value);
                }
                else if (property.Value is string)
                {
                    SetProperty(property.Key, (string)property.Value);
                }
                else if (property.Value.GetType().IsArray)
                {
                    if(property.Value is double[])
                    {
                        SetProperty(property.Key, (double[])property.Value);
                    }
                    //TODO
                }
                else throw new NotSupportedException("Property Type for key '" + property.Key + "' of type " + property.Value.GetType().FullName + " not supported");
            }
        }
        public RepPayload Clone()
        {
            return new RepPayload(OCPayloadInterop.OCRepPayloadClone(Handle));
        }
        public bool SetProperty(string name, long value)
        {
            return OCPayloadInterop.OCRepPayloadSetPropInt(Handle, name, value);
        }
        public bool TryGetInt64(string name, out long value)
        {
            return OCPayloadInterop.OCRepPayloadGetPropInt(Handle, name, out value);
        }

        public bool SetPropertyNull(string name)
        {
            return OCPayloadInterop.OCRepPayloadSetNull(Handle, name);
        }
        public bool TryGetPropertyIsNull(string name, out bool isNull)
        {
            isNull = OCPayloadInterop.OCRepPayloadIsNull(Handle, name);
            return true;
        }
        public bool SetProperty(string name, double value)
        {
            return OCPayloadInterop.OCRepPayloadSetPropDouble(Handle, name, value);
        }
        public bool TryGetDouble(string name, out double value)
        {
            return OCPayloadInterop.OCRepPayloadGetPropDouble(Handle, name, out value);
        }
        public bool SetProperty(string name, bool value)
        {
            return OCPayloadInterop.OCRepPayloadSetPropBool(Handle, name, value);
        }
        public bool TryGetBool(string name, out bool value)
        {
            return OCPayloadInterop.OCRepPayloadGetPropBool(Handle, name, out value);
        }
        public bool SetProperty(string name, string value)
        {
            return OCPayloadInterop.OCRepPayloadSetPropString(Handle, name, value);
        }
        public bool SetProperty(string name, double[] value)
        {
            return OCPayloadInterop.OCRepPayloadSetDoubleArray(Handle, name, value, new UIntPtr[] { (UIntPtr)value.Length });
        }
        public bool TryGetString(string name, out string value)
        {
            IntPtr ptr;
            value = null;
            bool result = OCPayloadInterop.OCRepPayloadGetPropString(Handle, name, out ptr);
            if (result)
            {
                value = Marshal.PtrToStringAnsi(ptr);
            }
            return result;
        }
        public bool SetUri(string uri)
        {
            return OCPayloadInterop.OCRepPayloadSetUri(Handle, uri);
        }
        public bool AddResourceType(string resourceType)
        {
            return OCPayloadInterop.OCRepPayloadAddResourceType(Handle, resourceType);
        }
        public void Append(RepPayload child)
        {
            OCPayloadInterop.OCRepPayloadAppend(Handle, child.Handle);
        }
    }
}
