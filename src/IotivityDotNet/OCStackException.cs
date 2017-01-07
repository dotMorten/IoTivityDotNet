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

        [System.Diagnostics.DebuggerHiddenAttribute]
        internal static void ThrowIfError(OCStackResult result, string message = null)
        {
            if (result == OCStackResult.OC_STACK_OK)
                return;
            Exception exception = new OCStackException(result, message);
            if (result == OCStackResult.OC_STACK_INVALID_PARAM)
                exception = new ArgumentException(message, exception);
            else if (result == OCStackResult.OC_STACK_INVALID_METHOD)
                exception = new InvalidOperationException(message, exception);
            else if (result == OCStackResult.OC_STACK_UNAUTHORIZED_REQ)
                exception = new UnauthorizedAccessException(message, exception);
            throw exception;
        }
    }
}
