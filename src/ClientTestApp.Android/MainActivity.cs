using Android.App;
using Android.Widget;
using Android.OS;
using IotivityDotNet.Interop;

namespace ClientTestApp.Android
{
    [Activity(Label = "ClientTestApp.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);



            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            TextView status = FindViewById<TextView>(Resource.Id.StatusTextView);

            try
            {

                //Uncomment this to test if the lib is loading correctly:
                Java.Lang.Runtime.GetRuntime().LoadLibrary("liboctbstack");

                var result = OCStack.OCInit(null, 0, OCMode.OC_CLIENT);
            }
            catch(System.Exception ex)
            {
                status.Text = ex.Message + "\n" + ex.StackTrace;
            }
            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

