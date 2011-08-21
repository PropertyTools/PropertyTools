cd ..\Packages
copy ..\source\PropertyTools.Wpf\bin\Release\*.dll PropertyTools.Wpf\lib
copy ..\source\PropertyTools.Wpf\bin\Release\*.XML PropertyTools.Wpf\lib
..\tools\nuget.exe pack PropertyTools.Wpf\PropertyTools.Wpf.nuspec