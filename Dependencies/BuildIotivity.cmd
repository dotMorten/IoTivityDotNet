@ECHO OFF
REM BUILD PREREQS:
REM Python 2.x (3.x won't work!) http://www.python.org
REM scons Windows Installer (into 2.x python when asked): http://scons.org/pages/download.html
REM 7-ZIP
REM CMake
REM Pull AllJoyn submodule.
REM At first build you might be asked to pull more repos

SET PATH=%PATH%;C:\Python27\;C:\Python27\Scripts;C:\Program Files\CMake\bin;C:\Program Files\7-Zip;c:\Program Files (x86)\Git\bin

CD iotivity
ECHO ********************************** BUILDING x86 Release **********************************
CALL scons TARGET_OS=windows TARGET_ARCH=x86 RELEASE=1 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=0 WITH_TCP=0 BUILD_SAMPLE=OFF LOGGING=OFF TEST=1 resource/csdk
MD ..\..\Libs\octbstack\Windows\x86\
COPY out\windows\x86\release\resource\csdk\octbstack.dll ..\..\Libs\octbstack\Windows\x86\ /Y

ECHO ********************************** BUILDING x64 Release **********************************
CALL scons TARGET_OS=windows TARGET_ARCH=amd64 RELEASE=1 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=0 WITH_TCP=0 BUILD_SAMPLE=OFF LOGGING=OFF TEST=1 resource/csdk
MD ..\..\Libs\octbstack\Windows\x64\
COPY out\windows\amd64\release\resource\csdk\octbstack.dll ..\..\Libs\octbstack\Windows\x64\ /Y

ECHO ********************************** BUILDING x86 Debug **********************************
CALL scons TARGET_OS=windows TARGET_ARCH=x86 RELEASE=0 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=0 WITH_TCP=0 BUILD_SAMPLE=OFF LOGGING=OFF TEST=1 resource/csdk
MD ..\..\Libs\octbstack\Windows\x86-debug\
COPY out\windows\x86\debug\resource\csdk\octbstack.dll ..\..\Libs\octbstack\Windows\x86-debug\ /Y

ECHO ********************************** BUILDING x64 Debug **********************************
CALL scons TARGET_OS=windows TARGET_ARCH=amd64 RELEASE=0 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=0 WITH_TCP=0 BUILD_SAMPLE=OFF LOGGING=OFF TEST=1 resource/csdk
MD ..\..\Libs\octbstack\Windows\x64-debug\
COPY out\windows\amd64\debug\resource\csdk\octbstack.dll ..\..\Libs\octbstack\Windows\x64-debug\ /Y

REM Note: To build arm you need to fix SCONS
REM Note2: This doesn't work though. Builds OK but creates invalid binary
REM In C:\Python27\Lib\site-packages\scons-2.5.1\SCons\Tool\MSCommon\vc.py change
REM Add the following entry to '_ARCH_TO_CANONICAL':     "arm"		: "x86",
REM scons TARGET_OS=windows TARGET_ARCH=arm RELEASE=0 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=0 WITH_TCP=0 BUILD_SAMPLE=OFF LOGGING=OFF TEST=0 resource/csdk
REM scons TARGET_OS=windows TARGET_ARCH=arm RELEASE=1 WITH_RA=0 TARGET_TRANSPORT=IP SECURED=0 WITH_TCP=0 BUILD_SAMPLE=OFF LOGGING=OFF TEST=0 resource/csdk
PAUSE