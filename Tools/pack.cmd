mkdir ..\Packages\PropertyTools.Wpf\lib

copy ..\Output\PropertyTools.dll ..\Packages\PropertyTools.Wpf\lib
copy ..\Output\PropertyTools.XML ..\Packages\PropertyTools.Wpf\lib
copy ..\Output\PropertyTools.pdb ..\Packages\PropertyTools.Wpf\lib

copy ..\Output\PropertyTools.Wpf.dll ..\Packages\PropertyTools.Wpf\lib
copy ..\Output\PropertyTools.Wpf.XML ..\Packages\PropertyTools.Wpf\lib
copy ..\Output\PropertyTools.Wpf.pdb ..\Packages\PropertyTools.Wpf\lib

copy ..\license.txt ..\Packages\PropertyTools.Wpf

nuget.exe pack ..\Packages\PropertyTools.Wpf\PropertyTools.Wpf.nuspec -OutputDirectory ..\Packages > pack.log
