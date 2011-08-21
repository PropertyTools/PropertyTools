cd ..\Source\Examples
mkdir ..\..\Output
mkdir ..\..\Output\Examples
del /S /Q ..\..\Output\Examples\*.*
for /D %%G in (*) DO (
mkdir ..\..\Output\Examples\%%G
xcopy %%G\bin\Release\*.* ..\..\Output\Examples\%%G /S /Y
)
cd ..\..\Output\Examples
del /S *.pdb 
del /S *.vshost.exe 
del /S *.manifest 
del /S *.config
"C:\Program Files\7-Zip\7z.exe" a -r ..\PropertyTools-Examples.zip *.*
explorer ..
cd ..\..\Tools