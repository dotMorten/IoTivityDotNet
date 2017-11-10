using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestApp
{
    internal class Log
    {
        public static void WriteLine(string message, params object[] args)
        {
            OnLogEvent?.Invoke(null, string.Format(message, args:args));
        }

        public static event EventHandler<string> OnLogEvent;
    }
}
