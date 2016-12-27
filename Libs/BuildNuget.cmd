RMDIR nuget /S /Q
XCOPY dotMorten.Iotivity.CAPI.nuspec .\nuget\ /Y /F

echo f | XCOPY /Y /F dotMorten.Iotivity.CAPI-MonoAndroid10.targets .\nuget\build\MonoAndroid10\dotMorten.Iotivity.CAPI.targets
XCOPY octbstack\Android\x86\liboctbstack.so nuget\libs\MonoAndroid10\x86\ /Y /S
XCOPY octbstack\Android\armeabi-v7a\liboctbstack.so nuget\libs\MonoAndroid10\armeabi-v7a\ /Y /S
XCOPY octbstack\Android\x86-debug\liboctbstack.so nuget\libs\MonoAndroid10\x86-debug\ /Y /S
XCOPY octbstack\Android\armeabi-v7a-debug\liboctbstack.so nuget\libs\MonoAndroid10\armeabi-v7a-debug\ /Y /S

echo f | XCOPY /Y /F dotMorten.Iotivity.CAPI-net452.targets .\nuget\build\net452\dotMorten.Iotivity.CAPI.targets
REM XCOPY octbstack\Windows\x64\octbstack.dll nuget\libs\net452\x64\ /Y /S
REM XCOPY octbstack\Windows\x86\octbstack.dll nuget\libs\net452\x86\ /Y /S

XCOPY octbstack\Windows\x64\octbstack.dll nuget\runtimes\win10-x64\native\ /Y /S
XCOPY octbstack\Windows\x86\octbstack.dll nuget\runtimes\win10-x86\native\ /Y /S
XCOPY octbstack\Windows\arm\octbstack.dll nuget\runtimes\win10-arm\native\ /Y /S

XCOPY octbstack\Darwin\x86_64\release\liboctbstack.a nuget\staticlib\Xamarin.iOS10\ /Y /S

NUGET PACK nuget\dotMorten.Iotivity.CAPI.nuspec

PAUSE