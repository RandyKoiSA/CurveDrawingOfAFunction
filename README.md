# Repository Name: Curve-Drawing-of-Cos-2t-
*********************************************************
Language: C# program
*********************************************************
Distribution: Xubuntu
*********************************************************
Software: Monodevelop

Date of Creation: 11/27/2017

Author: Randy Le

Author's Email: 97Randy.le@gmail.com

Course: CPSC223N
*********************************************************
Source Files:
1. curvedrawing_3main.cs
2. curvedrawing_3frame.cs
3. curvedrawing_3logic.cs
*********************************************************
To run the program on linux please run this program in order in the terminal
1) mcs -target:library curvedrawing_3logic.cs -r:System.Drawing.dll -out:curvedrawing_3logic.dll
2) mcs -target:library curvedrawing_3frame.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:curvedrawing_3logic.dll -out:curvedrawing_3frame.dll
3) mcs curvedrawing_3main.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:curvedrawing_3frame.dll -r:curvedrawing_3logic.dll -out:curvedrawing3.exe
4) ./curvedrawing3.exe

or 

run the run.sh file with the command
1)sh run.sh

*********************************************************
A C# program that produces a graphical output. Drawing a curve using a given math
equations of that curve. Drawing a function that graphs a fascinating shape such as a
flower, figure-eights, hearts, snow flakes, spirals, diamonds, and other amazing patterns.

This program teaches.
1. The C# coordinate system and Math coordinate system.
2. Conversion between C# coordinates and Math coordinates.
3. The scale factor that relates the two types of coordinates.
4. Equations in polar coordinates; equations in rectangular coordinates.
5. How to add consant to the current image without destroying the existing content.

The program will have a UI that display a panel structure of buttons (Start, Pause/Resume/ Exit).
It also displays the X and Y coordinates values showned in mathematical coordiantes of the point
of the ball pen as it draws the curve.
