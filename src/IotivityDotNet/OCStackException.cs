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

#if !DEBUG
        [System.Diagnostics.DebuggerHiddenAttribute]
#endif
        internal static void ThrowIfError(OCStackResult result, string message = null)
        {
            var err = CreateException(result, message);
            if (err != null)
                throw err;
        }

        internal static Exception CreateException(OCStackResult result, string message = null)
        {
            if (result == OCStackResult.OC_STACK_OK)
                return null;
            Exception exception = new OCStackException(result, message);
            if (result == OCStackResult.OC_STACK_INVALID_PARAM)
                return new ArgumentException(message, exception);
            else if (result == OCStackResult.OC_STACK_INVALID_METHOD)
                return new InvalidOperationException(message, exception);
            else if (result == OCStackResult.OC_STACK_UNAUTHORIZED_REQ)
                return new UnauthorizedAccessException(message, exception);
            return exception;
        }
    }
}
