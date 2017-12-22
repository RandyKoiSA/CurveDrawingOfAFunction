/* Author: Randy Le
 * Author's Email: Randy.l5933@csu.fullerton.edu
 * Course: CPSC 223N
 * 
 * Source Files:
 * 1.curvedrawing_3main.cs
 * 2.curvedrawing_3frame.cs
 * 3.curvedarwing_3logic.cs
 * 4.run.sh
 * 
 * Pupose of this entire program:
 * Tracing and following the function : cos(2t). Having to change polar coordinatess to cartesian coordinates
 * Compile this file:
 * mcs curvedrawing_3main.cs -r:System.Windows.Form.dll -r:System.Drawing.dll -r:curvedrawing_3main.dll -r:curvedrawing_3logic.dll -out:curvedrawing3.exe
 * 
 * ./curvedrawing3.exe
 */
using System;
using System.Windows.Forms;

public class curvedrawing_3main{
    public static void Main(){
        System.Console.WriteLine("The curvedrawing program has initiated");
        curvedrawing_3frame program = new curvedrawing_3frame();
        Application.Run(program);
        System.Console.WriteLine("The curvedrawing program has closed");
    }
}