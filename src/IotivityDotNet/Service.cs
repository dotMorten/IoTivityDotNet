using IotivityDotNet.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IotivityDotNet
{
    public enum ServiceMode
    {
        Client = OCMode.OC_CLIENT,
        Server = OCMode.OC_SERVER,
        ClientServer = OCMode.OC_CLIENT_SERVER,
        Gateway = OCMode.OC_GATEWAY          /**< Client server mode along with routing capabilities.*/
    }

    public static class Service
    {
        private static CancellationTokenSource ct;
        private static TaskCompletionSource<object> tcs;
        private static OCEntityHandler globalHandler;
        private static StorageHandler storageHandler;
        private static GCHandle storageHandle;
        public static void Initialize(ServiceMode mode, string dataPath = "")
        {
            storageHandler = new StorageHandler(dataPath);
            storageHandle = GCHandle.Alloc(storageHandler);
            var fileresult = OCStack.OCRegisterPersistentStorageHandler(storageHandler.Handle);
            
            OCStackException.ThrowIfError(fileresult, "Failed to create storage handler");
            
            //var result = OCStack.OCInit(null, 0, (OCMode)mode);
            var result = OCStack.OCInit2((OCMode)mode, OCTransportFlags.OC_DEFAULT_FLAGS, OCTransportFlags.OC_DEFAULT_FLAGS, OCTransportAdapter.OC_ADAPTER_IP);
            // result = OCStack.OCInit("0.0.0.0", 0, (OCMode)mode);
            OCStackException.ThrowIfError(result);
            globalHandler = OCDefaultDeviceEntityHandler;
            GC.KeepAlive(storageHandler);
            result = OCStack.OCSetDefaultDeviceEntityHandler(globalHandler, IntPtr.Zero);
            OCStackException.ThrowIfError(result, "Failed to send to resource");


            ct = new CancellationTokenSource();
            tcs = new TaskCompletionSource<object>();

            Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    var result2 = OCStack.OCProcess();
                    if (result2 != OCStackResult.OC_STACK_OK)
                    {
                        // tcs.SetException(new Exception("OCStackException on Process: " + result2.ToString()));
                        // break;
                    }
                    await Task.Delay(1);
                }
                tcs.SetResult(true);
                tcs = null;
                ct = null;
            });
        }

        //public static byte[] DeviceId
        //{
        //    get
        //    {
        //        OCUUIdentity identity = null;
        //        var result = OCStack.OCGetDeviceId(out identity);
        //        if (result != OCStackResult.OC_STACK_OK)
        //        {
        //            throw new Exception(result.ToString());
        //        }
        //        return identity.deviceId;
        //    }
        //    set
        //    {
        //        if (value == null || value.Length > 16)
        //        {
        //            throw new ArgumentException(nameof(value));
        //        }
        //        var result = OCStack.OCSetDeviceId(new OCUUIdentity() { deviceId = value });
        //        if (result != OCStackResult.OC_STACK_OK)
        //        {
        //            throw new Exception(result.ToString());
        //        }
        //    }
        //}

        /// <summary>
        /// This function sets device information.
        /// Upon call to OCInit, the default Device Type (i.e. "rt") has already been set to the default
        /// Device Type "oic.wk.d". You do not have to specify "oic.wk.d" in the OCDeviceInfo.types linked
        /// list. The default Device Type is mandatory and always specified by this Device as the first
        /// Device Type.
        /// </summary>
        public static void SetDeviceInfo(string deviceName, IEnumerable<string> types, string specVersion, IEnumerable<string> dataModelVersions)
        {
            var octypes = OCStringLL.Create(types);
            var ocdataModelVersions = OCStringLL.Create(dataModelVersions);
            var info = new OCDeviceInfo()
            {
                deviceName = deviceName,
                types = octypes,
                specVersion = specVersion,
                dataModelVersions = ocdataModelVersions
            };
            var result = OCStack.OCSetDeviceInfo(info);
            OCStackException.ThrowIfError(result);
        }

        private static OCEntityHandlerResult OCDefaultDeviceEntityHandler(OCEntityHandlerFlag flag, OCEntityHandlerRequest entityHandlerRequest, IntPtr callbackParam)
        {
            return OCEntityHandlerResult.OC_EH_OK;
        }

        public static async Task Shutdown()
        {
            if (tcs == null) return;
            ct.Cancel();
            await tcs.Task;
            var result = OCStack.OCStop();
            OCStackException.ThrowIfError(result);
        }

        internal class StorageHandler
        {
            private FileOpenDelegate openDelegate;
            private FileCloseDelegate closeDelegate;
            private FileReadDelegate readDelegate;
            private FileUnlinkDelegate unlinkDelegate;
            private FileWriteDelegate writeDelegate;
            private GCHandle[] pinnedDelegates;
            private GCHandle pHandle;
            public OCPersistentStorage Handle { get; }
            //public OCPersistentStorage2 Handle2 { get; }
            private string _dataPath;
            public StorageHandler(string dataPath)
            {
                _dataPath = dataPath;
                openDelegate =new FileOpenDelegate(FileOpen);
                readDelegate = new FileReadDelegate(FileRead);
                closeDelegate = new FileCloseDelegate(FileClose);
                unlinkDelegate = new FileUnlinkDelegate(FileUnlink);
                writeDelegate = new FileWriteDelegate(FileWrite);
                var h1 = GCHandle.Alloc(openDelegate, GCHandleType.Normal);
                var h2 = GCHandle.Alloc(readDelegate, GCHandleType.Normal);
                var h3 = GCHandle.Alloc(writeDelegate, GCHandleType.Normal);
                var h4 = GCHandle.Alloc(closeDelegate, GCHandleType.Normal);
                var h5 = GCHandle.Alloc(unlinkDelegate, GCHandleType.Normal);
                pinnedDelegates = new GCHandle[] { h1, h2, h3, h4, h5 };

                //Handle = new OCPersistentStorage()
                //{
                //    Open = openDelegate,
                //    Read = readDelegate,
                //    Write = writeDelegate,
                //    Close = closeDelegate,
                //    Unlink = unlinkDelegate,
                //};
                Handle = new OCPersistentStorage()
                {
                    Open =   Marshal.GetFunctionPointerForDelegate(openDelegate),
                    Read =   Marshal.GetFunctionPointerForDelegate(readDelegate),
                    Write =  Marshal.GetFunctionPointerForDelegate(writeDelegate),
                    Close =  Marshal.GetFunctionPointerForDelegate(closeDelegate),
                    Unlink = Marshal.GetFunctionPointerForDelegate(unlinkDelegate),
                };
                pHandle = GCHandle.Alloc(Handle);
            }
            ~StorageHandler()
            {
                pHandle.Free();
                foreach (var item in pinnedDelegates)
                    item.Free();
                foreach (var item in streams)
                    item.Value.Dispose();
            }
            private Dictionary<IntPtr, System.IO.Stream> ActiveStreams = new Dictionary<IntPtr, System.IO.Stream>();
           
            int id = 0;
            Dictionary<IntPtr, System.IO.FileStream> streams = new Dictionary<IntPtr, System.IO.FileStream>();
            public IntPtr FileOpen(string path, string mode)
            {
                System.IO.FileMode fmode = System.IO.FileMode.Create;
                System.IO.FileAccess access = System.IO.FileAccess.Read;
                switch (mode)
                {
                    case "r":
                    case "rb":
                    case "r+":
                    case "rb+":
                        fmode = System.IO.FileMode.Open; break;
                    case "a":
                    case "a+":
                    case "ab":
                    case "ab+":
                        fmode = System.IO.FileMode.Append; break;
                    case "w+":
                    case "w":
                    case "wb":
                        fmode = System.IO.FileMode.Create; break;
                }
                if (mode.Contains("+") || mode.Contains("a"))
                    access = System.IO.FileAccess.ReadWrite;
                else if (mode.Contains("r"))
                    access = System.IO.FileAccess.Read;
                else if (mode.Contains("w"))
                    access = System.IO.FileAccess.ReadWrite;

                if (System.IO.File.Exists(path))
                {
                }
                else if (fmode == System.IO.FileMode.Open)
                {
                    return IntPtr.Zero;
                }

                path = System.IO.Path.Combine(_dataPath, path);
                var fs = System.IO.File.Open(path, fmode, access);
                id++;
                var ptr = new IntPtr(id);
                streams[ptr] = fs;
                return ptr;
            }
            public UIntPtr FileRead(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr file)
            {
                int len = (int)size * (int)nmemb;
                byte[] buff = new byte[len];
                int cnt = streams[file].Read(buff, 0, len);
                Marshal.Copy(buff, 0, data, cnt);
                return (UIntPtr)(cnt / (int)size);
            }
            public UIntPtr FileWrite(IntPtr data, UIntPtr size, UIntPtr nmemb, IntPtr file)
            {
                int len = (int)size * (int)nmemb;
                byte[] buff = new byte[len];
                Marshal.Copy(data, buff, 0, (int)len);
                streams[file].Write(buff, 0, (int)len);
                streams[file].Flush();
                return nmemb;
            }
            public int FileClose(IntPtr file)
            {
                streams[file].Dispose();
                streams.Remove(file);
                return 0;
            }
            public int FileUnlink(string path)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return 1;
                }
                return 0;
            }
        }
    }
}
