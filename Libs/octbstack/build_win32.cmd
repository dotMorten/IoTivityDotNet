SET PATH=%PATH%;C:\Python27\;C:\Python27\Scripts;C:\Program Files\CMake\bin;C:\Program Files\7-Zip;c:\Program Files (x86)\Git\bin
CD ..\..\Dependencies\Iotivity

scons TARGET_OS=windows TARGET_ARCH=x86 RELEASE=1 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=1 WITH_TCP=0 BUILD_SAMPLE=ON LOGGING=OFF TEST=1
xcopy out\windows\x86\release\octbstack.dll ..\..\Libs\octbstack\Win7\x86\ /Y /S

scons TARGET_OS=windows TARGET_ARCH=amd64 RELEASE=1 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=1 WITH_TCP=0 BUILD_SAMPLE=ON LOGGING=OFF TEST=1
xcopy out\windows\amd64\release\octbstack.dll ..\..\Libs\octbstack\Win7\x64\ /Y /S

scons TARGET_OS=windows TARGET_ARCH=x86 RELEASE=0 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=1 WITH_TCP=0 BUILD_SAMPLE=ON LOGGING=OFF TEST=1
xcopy out\windows\x86\debug\octbstack.dll ..\..\Libs\octbstack\Win7\x86-debug\ /Y /S

scons TARGET_OS=windows TARGET_ARCH=amd64 RELEASE=0 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=1 WITH_TCP=0 BUILD_SAMPLE=ON LOGGING=OFF TEST=1
xcopy out\windows\amd64\debug\octbstack.dll ..\..\Libs\octbstack\Win7\x64-debug\ /Y /S
