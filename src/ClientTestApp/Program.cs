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
            IotivityNet.Service.Initialize(IotivityNet.ServiceMode.ClientServer);
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

            IotivityNet.Service.Shutdown().Wait();
        }
    }
}
