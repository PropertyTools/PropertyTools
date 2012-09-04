set dir=..\Output\NET40\Examples\
del /S /Q %dir%\*.pdb 
del /S /Q %dir%\*.vshost.exe 
del /S /Q %dir%\*.manifest 
del /S /Q %dir%\*.config

set dir=..\Output\NET45\Examples\
del /S /Q %dir%\*.pdb 
del /S /Q %dir%\*.vshost.exe 
del /S /Q %dir%\*.manifest 
del /S /Q %dir%\*.config

"C:\Program Files\7-Zip\7z.exe" a -r ..\Output\PropertyTools-%1.zip ..\Output\*.* > ZipRelease.log