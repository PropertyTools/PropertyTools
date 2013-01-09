for %%G in (..\Packages\*.nupkg) do ..\Tools\NuGet\nuget.exe push %%G %NUGET_ACCESS_KEY% > push.log
