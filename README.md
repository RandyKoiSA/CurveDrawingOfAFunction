# Repository Name: Curve-Drawing-Of-A-Function
A C# program that produces a graphical output. Drawing a curve using a given math
equations of that curve. Drawing a function that graphs a fascinating shape such as a
flower, figure-eights, hearts, snow flakes, spirals, diamonds, and other amazing patterns.
## Info
Author: Randy Le <br>
Author's Email: 97Randy.le@gmail.com <br>

Course: CPSC223N <br>
Date of Creation: 11/27/2017 <br>

Language: C# program <br>
Distribution: Xubuntu <br>
Software: Monodevelop <br>
## Contents
1. curvedrawing_3main.cs <br>
2. curvedrawing_3frame.cs <br>
3. curvedrawing_3logic.cs <br>
## Setup and Installation
To run the program on linux please run the follow in the terminal to create the curvedrawing_3logic.dll:
```
  $ mcs -target:library curvedrawing_3logic.cs -r:System.Drawing.dll -out:curvedrawing_3logic.dll
```
After, run the following in the terminal to create the curvedrawing_3frame.dll:
```
  $ mcs -target:library curvedrawing_3frame.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:curvedrawing_3logic.dll -
```
Also, run the following to create an .exe file:
```
  $ mcs curvedrawing_3main.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:curvedrawing_3frame.dll -r:curvedrawing_3logic.dll -out:curvedrawing3.exe
```
Finally, execute the following:
```
  $ ./curvedrawing3.exe
```
or run the run.sh file with the command
```
  $ sh run.sh
```

## What I have learned
1. The C# coordinate system and Math coordinate system.
2. Conversion between C# coordinates and Math coordinates.
3. The scale factor that relates the two types of coordinates.
4. Equations in polar coordinates; equations in rectangular coordinates.
5. How to add consant to the current image without destroying the existing content.

The program will have a UI that display a panel structure of buttons (Start, Pause/Resume/ Exit).
It also displays the X and Y coordinates values showned in mathematical coordiantes of the point
of the ball pen as it draws the curve.
