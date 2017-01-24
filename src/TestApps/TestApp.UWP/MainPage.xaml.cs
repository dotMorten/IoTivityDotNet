using ClientTestApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Server server;
        private Client client;

        public MainPage()
        {
            this.InitializeComponent();

            Log.OnLogEvent += (s, e) => { var _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { logOutput.Text += e + "\n"; }); };
            Log.WriteLine("Initializing OCSTACK...");
            IotivityDotNet.Service.Initialize(IotivityDotNet.ServiceMode.ClientServer, Windows.Storage.ApplicationData.Current.LocalFolder.Path);
            Log.WriteLine("Initialized");

            Log.WriteLine("Creating devices...");
            server = new Server();
            Log.WriteLine("Starting client...");
            client = new Client();
            Log.WriteLine("Ready");
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            client.Dispose();
            server.Dispose();

            var _ = IotivityDotNet.Service.Shutdown();
            base.OnNavigatingFrom(e);
        }
    }
}
