### Build Instructions

First make sure the [iotivity submodule](https://github.com/dotMorten/IoTivity) is cloned into Dependencies\iotivity


### Prerequisites

Install Python 2.x (3.x won't work!) http://www.python.org

Install SCONS
 - Windows Installer (into 2.x python when asked): http://scons.org/pages/download.html

Install 7-ZIP (build script is set for the x64 version, so install that, or update path in build script)

From a command prompt execute `\Dependencies\BuildIotivity.cmd`
The first build you might be asked to pull more repos. Just run the git command you're told to clone, then rerun the `BuildIotivity` script

Once built, you can open, build and run `\src\IotivityDotNet.sln`

Only Windows and WinUWP (x64 and x86) is currently working. WinUWP ARM and Xamarin (Android/iOS) support is still in the works (Help wanted)
