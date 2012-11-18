mkdir ..\Packages\PropertyTools.Wpf\lib
mkdir ..\Packages\PropertyTools.Wpf\lib\NET40
mkdir ..\Packages\PropertyTools.Wpf\lib\NET45

copy ..\Output\NET40\PropertyTools.??? ..\Packages\PropertyTools.Wpf\lib\NET40
copy ..\Output\NET40\PropertyTools.Wpf.??? ..\Packages\PropertyTools.Wpf\lib\NET40

copy ..\Output\NET45\PropertyTools.??? ..\Packages\PropertyTools.Wpf\lib\NET45
copy ..\Output\NET45\PropertyTools.Wpf.??? ..\Packages\PropertyTools.Wpf\lib\NET45

copy ..\license.txt ..\Packages\PropertyTools.Wpf

set EnableNuGetPackageRestore=true

..\Tools\NuGet\nuget.exe pack ..\Packages\PropertyTools.Wpf\PropertyTools.Wpf.nuspec -OutputDirectory ..\Packages > pack.log
