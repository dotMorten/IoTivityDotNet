using IotivityDotNet.Interop;
using System;

namespace IotivityDotNet
{
    public class OCStackException : Exception
    {
        private OCStackException(OCStackResult result, string message) : base(GetMessage(result, message))
        {
            ErrorCode = result;
        }

        public OCStackResult ErrorCode { get; }

        private static string GetMessage(OCStackResult result, string message = null)
        {
            if(string.IsNullOrEmpty(message))
                return result.ToString();
            return message + " (" + result.ToString() + ")";
        }

        internal static void ThrowIfError(OCStackResult result, string message = null)
        {
            if (result == OCStackResult.OC_STACK_OK)
                return;
            throw new OCStackException(result, message);
        }
    }
}
