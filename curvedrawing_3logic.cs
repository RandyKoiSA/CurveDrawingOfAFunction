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
 * mcs target:library curvedrawing_3logic.cs -r:System.Drawing.dll -out:curvedrawing_3logic.dll
 */
public class curvedrawing_3logic{
    private double magnitude_of_tangent_vector_squared;
    private double magnitude_of_tangent_vector;

    public void get_next_coordinates(double distance_in_1_tic, ref double t, out double x, out double y){
        magnitude_of_tangent_vector_squared = 4 + System.Math.Cos(t) * System.Math.Cos(t);
        magnitude_of_tangent_vector = System.Math.Sqrt(magnitude_of_tangent_vector_squared);
        t = t + distance_in_1_tic / magnitude_of_tangent_vector;
        x = System.Math.Cos(2 * t) * System.Math.Cos(t);
        y = System.Math.Cos(2 * t) * System.Math.Sin(t);
    }
}


//To: Students in the 223N class
//This file contains an implementation of the get_next_coordinates method used with the curve defined by r = a+b*t, which has corresponding cartesian
//coordinate functions: x = r*cos(t) = (a+b*t)*cos(t) and y = r*sin(t) = (a+b*t)*sin(t).

//The tangent vector is r'(t) = (x'(t),y'(t)), and the magnitude_of_tangent_vector_squared = x'(t)^2+y'(t)^2 = b^2+(a+b*t)^2.
//That is the tangent vector that corresponds to the initial curve function r = a+b*t.
//You can not use the same code as found in this get_next_coordinates function because it only works with the curve: r=a+b*t.
//If you use this get_next_coordinates method with your curve function you will create a wrong program, and thereby loose a lot of points.


//You have to do the hard steps yourself:
//1. Start with your curve function r(t) = (x(t),y(t))
//2. Find the derivatives x'(t), y'(t)
//3. Find the tangent vector (x'(t),y'(t))
//4. Compute the absolute value of that tangent vector which is also known as magnitude of the tangent vector
//5. Use that magnitude as the divisor as demonstrated in the function in the class Spiral_algorithms.
