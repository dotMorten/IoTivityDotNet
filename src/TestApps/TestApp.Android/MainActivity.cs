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
                RunOnUiThread(() =>
                {
                    status.Text += e +"\n";
                });
            };

            button.Click += delegate
            {
                button.Enabled = false;
                try
                {
                    status.Text = "Initializing IoTivity in Client service mode...\n";
                    IotivityDotNet.Service.Initialize(IotivityDotNet.ServiceMode.Client);
                    status.Text += "IoTivity initialized!\n";
                    client = new ClientTestApp.Client();

                }
                catch (System.Exception ex)
                {
                    status.Text += "IOTIVITY ERROR : " + ex.Message + "\n" + ex.StackTrace + "\n";
                    button.Enabled = true;
                }
            };
        }
    }
}

