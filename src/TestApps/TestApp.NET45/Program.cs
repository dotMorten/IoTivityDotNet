using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestApp
{
    class Program
    {

        static Server server;
        static Client client;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            EnableAggressiveGC();
            IotivityDotNet.Service.Initialize(IotivityDotNet.ServiceMode.ClientServer);
            IotivityDotNet.Service.SetDeviceInfo(".NET Console Test App", new string[] { "oic.wk.d" }, null, null);
            Log.OnLogEvent += (s, e) => Console.WriteLine(e);

            Console.WriteLine("Creating devices...");
            server = new Server();

            Console.WriteLine("Creating client...");
            client = new Client();

            Console.WriteLine("Service running. Press any key to close\n");

            var key = Console.ReadKey();
            Console.WriteLine("Shutting down...");
            client.Dispose();
            server.Dispose();

            IotivityDotNet.Service.Shutdown().Wait();
            running = false;
        }

        //Loop for stressing the GC to ensure handles are released (don't do this outside testing!)
        static bool running = true;
        private static void EnableAggressiveGC()
        {
            Task.Run(async () =>
            {
                while (running)
                {
                    await Task.Delay(1000);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                };
            });
        }
    }
}
