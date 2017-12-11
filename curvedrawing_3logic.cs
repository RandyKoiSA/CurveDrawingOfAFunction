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