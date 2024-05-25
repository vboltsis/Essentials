using System.Numerics;

namespace FeatureExamples;

/*
The Vector<float> constructor is expecting an array of a certain size.
The System.Numerics.Vector<T> structure is a SIMD-enabled type,
that is designed to efficiently perform operations on multiple elements at once.
The size of a Vector<T> instance is determined by the hardware and can be different on different machines.

To fix any errors, you can check the Vector<float>.Count property
to find out the size of a Vector<float> on your machine and adjust the size of your array accordingly.
If the size is larger than the array you use, you can pad your array with zeros to match the required size. 
*/

public class VectorExample
{
    public static void Example()
    {
        var v1 = new Vector<float>(new float[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        var v2 = new Vector<float>(new float[] { 9, 10, 11, 12, 13, 14, 15, 16 });

        // Addition
        var addResult = v1 + v2;

        // Subtraction
        var subResult = v1 - v2;

        // Multiplication
        var mulResult = v1 * v2;

        // Division
        var divResult = v1 / v2;

        // Bitwise And
        var andResult = v1 & v2;

        // Bitwise Or
        var orResult = v1 | v2;

        // Greater Than
        var gtResult = Vector.GreaterThan(v1, v2);

        // Absolute value
        var absResult = Vector.Abs(v1);

        // Square Root
        var sqrtResult = Vector.SquareRoot(v1);

        Console.WriteLine("Addition Result: " + addResult);
        Console.WriteLine("Square Root Result: " + sqrtResult);
    }
}
