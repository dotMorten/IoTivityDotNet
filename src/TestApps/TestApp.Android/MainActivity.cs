using Android.App;
using Android.Widget;
using Android.OS;

namespace TestApp.Android
{
    [Activity(Label = "TestApp.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private ClientTestApp.Client client;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            TextView status = FindViewById<TextView>(Resource.Id.StatusTextView);

            ClientTestApp.Log.OnLogEvent += (s, e) =>
            {
                status.Text += e;
            };

            button.Click += delegate
            {
                button.Enabled = false;
                try
                {
                    status.Text = "Initializing IoTivity in Client service mode...";
                    IotivityDotNet.Service.Initialize(IotivityDotNet.ServiceMode.Client);
                    status.Text += "\nIoTivity initialized!";
                    client = new ClientTestApp.Client();

                }
                catch (System.Exception ex)
                {
                    status.Text += "\nIOTIVITY ERROR : " + ex.Message + "\n" + ex.StackTrace;// +
                   //"\IOTIVITY: 0x" + ex.AllJoynErrorCode.ToString("x4") + " " + ex.AllJoynError
                   //+ "\n\t" + ex.AllJoynComment;
                     button.Enabled = true;
                }
            };
        }
    }
}

