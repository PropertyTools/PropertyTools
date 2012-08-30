mkdir ..\Packages\PropertyTools.Wpf\lib
mkdir ..\Packages\PropertyTools.Wpf\lib\NET40

set dest=..\Packages\PropertyTools.Wpf\lib\NET40

copy ..\Output\PropertyTools.dll %dest%
copy ..\Output\PropertyTools.XML %dest%
copy ..\Output\PropertyTools.pdb %dest%

copy ..\Output\PropertyTools.Wpf.dll %dest%
copy ..\Output\PropertyTools.Wpf.XML %dest%
copy ..\Output\PropertyTools.Wpf.pdb %dest%

copy ..\license.txt ..\Packages\PropertyTools.Wpf

set EnableNuGetPackageRestore=true

..\Tools\NuGet\nuget.exe pack ..\Packages\PropertyTools.Wpf\PropertyTools.Wpf.nuspec -OutputDirectory ..\Packages > pack.log
