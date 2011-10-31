cd ..\Packages
copy ..\Output\PropertyTools.Wpf.dll PropertyTools.Wpf\lib
copy ..\Output\PropertyTools.Wpf.XML PropertyTools.Wpf\lib
..\tools\nuget.exe pack PropertyTools.Wpf\PropertyTools.Wpf.nuspec
cd ..\Tools