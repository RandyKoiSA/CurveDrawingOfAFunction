echo First remove old binary files
rm *.dll
rm *.exe
echo view the list of source files
ls -ls
echo Compile the curvedrawing_3logic.cs to creat the file
mcs -target:library curvedrawing_3logic.cs -r:System.Drawing.dll -out:curvedrawing_3logic.dll
echo Compile the curvedrawing_3frame.cs to create the file
mcs -target:library curvedrawing_3frame.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:curvedrawing_3logic.dll -out:curvedrawing_3frame.dll
echo Compile the curvedrawing_3main.cs to create the file
mcs curvedrawing_3main.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:curvedrawing_3frame.dll -r:curvedrawing_3logic.dll -out:curvedrawing3.exe

./curvedrawing3.exe
echo the script has terminated