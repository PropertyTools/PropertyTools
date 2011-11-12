
copy ..\Output\PropertyTools.Wpf.dll ..\Packages\PropertyTools.Wpf\lib
copy ..\Output\PropertyTools.Wpf.XML ..\Packages\PropertyTools.Wpf\lib
nuget.exe pack ..\Packages\PropertyTools.Wpf\PropertyTools.Wpf.nuspec -OutputDirectory ..\Packages > pack.log
