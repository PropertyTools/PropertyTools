cd ..\Source
mkdir ..\Output
mkdir ..\Output\Release
copy ..\Source\PropertyTools.Wpf\bin\Release\*.dll ..\Output\Release
cd ..\Output\Release
"C:\Program Files\7-Zip\7z.exe" a -r ..\PropertyTools-Release.zip *.*
explorer ..
cd ..\..\Tools