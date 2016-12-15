# IoTivityDotNet

A .NET Wrapper around the IoTivity C SDK, and a higher-level API on top for simplicity.

**NOTE: WORK IN PROGRESS!**
- Only a limited set of features implemented for now for creating, discovering and controlling devices.
- Only Windows Win32 + UWP supported (however not on ARM yet)
- Xamarin support is in the works...
- Build generates .NET Standard 1.2 nuget packages but will only work with .NET 4.5.1+ and UWP (until I get the native packages needed to work).

####Usage

To use, first initialize the Iotivity service:

```csharp
 IotivityNet.Service.Initialize(IotivityNet.ServiceMode.Client);
```

You can then search for device resources using Device Discovery:

```csharp
var svc = new IotivityDotNet.DiscoverResource("/oic/res");
svc.ResourceDiscovered += (s, e) =>
{
    Console.WriteLine($"Device Discovered @ {e.Response.DeviceAddress}");
};
svc.Start();
```

When you're done, shut down the Iotivity service:

```csharp
await IotivityNet.Service.Shutdown();
```

See the .NET Console app for examples of creating devices (`Server.cs`) and finding/controlling devices (`Client.cs`)

Make sure you deploy the obtbstack.dll file in the x86 and x64 folders (See the test app, or use the generated nuget package).

##### Notes

UWP can't network loop-back and discover devices in its own process, so you can't run a client and a server in the same app. Try starting the .NET Console app which host devices, and then run the UWP app to discover them.
