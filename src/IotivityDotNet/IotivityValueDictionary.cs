using IotivityDotNet.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    internal class IotivityValueDictionary : IDictionary<string, object>
    {
        private IDictionary<string, object> _data;

        public IotivityValueDictionary()
        {
            _data = new Dictionary<string, object>();
        }

        internal IotivityValueDictionary(OCRepPayload ocpayload) : this()
        {
            foreach(var item in FromOCRepPayload(ocpayload))
            {
                Add(item.Key, item.Value);
            }
        }

        internal IotivityValueDictionary(IEnumerable<KeyValuePair<string, object>> values) : this()
        {
            if (values != null)
                foreach (var item in values)
                    Add(item);
        }

        internal static IEnumerable<KeyValuePair<string, object>> FromOCRepPayload(OCRepPayload ocpayload)
        {
            var values = ocpayload.values;
            while (values != IntPtr.Zero)
            {
                var payloadValue = Marshal.PtrToStructure<OCRepPayloadValue>(values);
                var value = payloadValue.value;
                switch (payloadValue.type)
                {
                    case OCRepPayloadPropType.OCREP_PROP_NULL:
                        yield return new KeyValuePair<string, object>(payloadValue.name, null); break;
                    case OCRepPayloadPropType.OCREP_PROP_STRING:
                        {
                            var strvalue = Marshal.PtrToStringAnsi(value.ocByteStr);
                            yield return new KeyValuePair<string, object>(payloadValue.name, strvalue); break;
                        }
                    case OCRepPayloadPropType.OCREP_PROP_BOOL:
                        {
                            bool bvalue = value.b;
                            yield return new KeyValuePair<string, object>(payloadValue.name, bvalue);
                            break;
                        }
                    case OCRepPayloadPropType.OCREP_PROP_DOUBLE:
                        {

                            double dvalue = value.d;
                            yield return new KeyValuePair<string, object>(payloadValue.name, dvalue);
                            break;
                        }
                    case OCRepPayloadPropType.OCREP_PROP_INT:
                        {
                            long lvalue = value.i;
                            yield return new KeyValuePair<string, object>(payloadValue.name, lvalue);
                            break;
                        }
                    case OCRepPayloadPropType.OCREP_PROP_ARRAY:
                        {
                            var arr = value.arr;
                            int size = (int)arr.dimensions0;
                            //var v = Marshal.PtrToStructure<OCRepPayloadValueArrayUnion>(arr.value);
                            switch(arr.type)
                            {
                                case OCRepPayloadPropType.OCREP_PROP_DOUBLE:
                                    double[] avalue = new double[size];
                                    Marshal.Copy(arr.value, avalue, 0, avalue.Length);
                                    yield return new KeyValuePair<string, object>(payloadValue.name, avalue);
                                    break;
                                case OCRepPayloadPropType.OCREP_PROP_INT:
                                    var alvalue = new long[size];
                                    Marshal.Copy(arr.value, alvalue, 0, alvalue.Length);
                                    yield return new KeyValuePair<string, object>(payloadValue.name, alvalue);
                                    break;
                                default:
                                    throw new NotImplementedException("Cannot convert " + arr.type.ToString() + " array");
                            }
                            //var a = Marshal.PtrToStructure<double[]>(arr);

                            break;
                        }
                    case OCRepPayloadPropType.OCREP_PROP_BYTE_STRING:
                    case OCRepPayloadPropType.OCREP_PROP_OBJECT:
                    default:
                        throw new NotImplementedException("Cannot convert " + payloadValue.type.ToString());
                }
                values = payloadValue.next;
            }
        }

        public object this[string key]
        {
            get { return _data[key]; }
            set
            {
                ValidateValue(value);
                _data[key] = value;
            }
        }

        internal void AssignToOCRepPayload(IntPtr ocRepPayloadHandle)
        {
            foreach (var property in this)
            {
                bool ok = false;
                if (property.Value == null)
                {
                    ok = OCPayloadInterop.OCRepPayloadSetNull(ocRepPayloadHandle, property.Key);
                }
                else if (property.Value is bool)
                {
                    ok = OCPayloadInterop.OCRepPayloadSetPropBool(ocRepPayloadHandle, property.Key, (bool)property.Value);
                }
                else if (property.Value is double)
                {
                    ok = OCPayloadInterop.OCRepPayloadSetPropDouble(ocRepPayloadHandle, property.Key, (double)property.Value);
                }
                else if (property.Value is long)
                {
                    ok = OCPayloadInterop.OCRepPayloadSetPropInt(ocRepPayloadHandle, property.Key, (long)property.Value);
                }
                else if (property.Value is string)
                {
                    ok = OCPayloadInterop.OCRepPayloadSetPropString(ocRepPayloadHandle, property.Key, (string)property.Value);
                }
                else if (property.Value is double[])
                {
                    var value = (double[])property.Value;
                    ok = OCPayloadInterop.OCRepPayloadSetDoubleArray(ocRepPayloadHandle, property.Key, value, new UIntPtr[] { (UIntPtr)value.Length });
                }
                else if (property.Value is long[])
                {
                    var value = (long[])property.Value;
                    ok = OCPayloadInterop.OCRepPayloadSetIntArray(ocRepPayloadHandle, property.Key, value, new UIntPtr[] { (UIntPtr)value.Length });
                }
                else if (property.Value is bool[])
                {
                    var value = (bool[])property.Value;
                    ok = OCPayloadInterop.OCRepPayloadSetBoolArray(ocRepPayloadHandle, property.Key, value, new UIntPtr[] { (UIntPtr)value.Length });
                }
                else
                    throw new NotSupportedException("Property Type for key '" + property.Key + "' of type " + property.Value.GetType().FullName + " not supported");
                if (!ok)
                    throw new InvalidOperationException($"Failed to assigning property '{property.Key}' to payload");

            }
        }

        private static void ValidateValue(object value)
        {
            // Simple types
            if (value == null || value is long || value is double || value is bool || value is string)
                return;

            // Throw helpful error for some common mistakes
            if(value is int || value is short || value is ushort || value is uint || value is ulong)
            {
                throw new ArgumentException($"Integer type '{value.GetType().FullName}' not supported. Use 'long' instead");
            }
            if (value is float || value is decimal)
            {
                throw new ArgumentException($"Floating point type '{value.GetType().FullName}' not supported. Use 'double' instead");
            }

            if (value is Array)
            {
                if (value is double[] || value is long[] || value is bool[])
                    return;
                // OCREP_PROP_ARRAY
                // TODO: Validate types in array
                foreach (var item in ((Array)value))
                    ValidateValue(item);
                return;
            }
            var typeinfo = value.GetType().GetTypeInfo();
            if (value is RepPayload)
            {
                // OCREP_PROP_OBJECT,
                return;
                // TODO: Go through properties, ensure empty constructor etc
            }
            if (value is IDictionary<string, object>)
            {
                // OCREP_PROP_OBJECT,
                if (value is IotivityValueDictionary)
                    return;
                foreach (var item in value as IDictionary<string, object>)
                    ValidateValue(item.Value);
                return;
            }
            // TODO:
            // OCREP_PROP_BYTE_STRING,

            throw new ArgumentException($"Type '{value.GetType().FullName}' not supported");
        }

        public int Count => _data.Count;

        public bool IsReadOnly => _data.IsReadOnly;

        public ICollection<string> Keys => _data.Keys;

        public ICollection<object> Values => _data.Values;

        public void Add(KeyValuePair<string, object> item)
        {
            ValidateValue(item.Value);
            _data.Add(item);
        }

        public void Add(string key, object value)
        {
            ValidateValue(value);
            _data.Add(key, value);
        }

        public void Clear() => _data.Clear();

        public bool Contains(KeyValuePair<string, object> item) => _data.Contains(item);

        public bool ContainsKey(string key) => _data.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _data.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _data.GetEnumerator();

        public bool Remove(KeyValuePair<string, object> item) => _data.Remove(item);

        public bool Remove(string key) => _data.Remove(key);

        public bool TryGetValue(string key, out object value) => _data.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_data).GetEnumerator();
    }
}
