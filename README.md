# IoTivityDotNet

A .NET Wrapper around the IoTivity C SDK, and a higher-level API on top for simplicity.

**NOTE: WORK IN PROGRESS!**
- Only a limited set of features implemented for now.
- Only Windows Win32 support.
- Library seems to work with UWP but network seems to be blocked.
- Xamarin support is in the works...
- Nuget packages with libraries for all platform supported also in the works.

####Usage

To use, first initialize the Iotivity service:

```csharp
 IotivityNet.Service.Initialize(IotivityNet.ServiceMode.Client);
```

You can then search for device resources using Device Discovery:

```csharp
var svc = new IotivityNet.OC.DiscoverResource("/oic/res");
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

Make sure you deploy the obtbstack.dll file in the x86 and x64 folders (See the test app).
