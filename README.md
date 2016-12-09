# IoTivityDotNet

A .NET Wrapper around the IoTivity C SDK, and a higher-level API on top for simplicity.

**NOTE: WORK IN PROGRESS!**

####Usage

To use, first initialize the Iotivity service:

```csharp
 IotivityNet.Service.Initialize(IotivityNet.ServiceMode.Client);
```

You can then search for devices using Device Discovery:

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
