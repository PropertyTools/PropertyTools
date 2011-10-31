cd ..\Output\Examples
del /S *.pdb 
del /S *.vshost.exe 
del /S *.manifest 
del /S *.config
"C:\Program Files\7-Zip\7z.exe" a -r ..\PropertyTools-Examples.zip *.*
explorer ..
cd ..\..\Tools