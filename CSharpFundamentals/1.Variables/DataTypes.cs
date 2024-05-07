using System.Diagnostics;

namespace CSharpFundamentals;

#region Notes
/*
In C#, the maximum value for a double precision floating point number (double) is 1.7976931348623157E+308.
The "E+308" in this representation is part of the scientific notation, which is a way to express very large or very small numbers succinctly.
The letter "E" in scientific notation stands for "exponent of ten."
Therefore, the notation "E+308" tells us that the number should be multiplied by 10^308
In other words, the number 1.7976931348623157E+308 is equivalent to 1.7976931348623157 multiplied by 10^308
So, if you were to write this number out in full (without scientific notation),
it would be 1.7976931348623157 followed by 308 zeros.
This representation makes it much easier to work with extremely large numbers,
such as the upper limit of a double in programming, without having to write out a vast number of zeros. 
*/
#endregion

public class DataTypes
{
    public void VariableTypes()
    {
        // Value types
        int number = 1; // -2,147,483,648 to 2,147,483,647
        short shortNumber = 1; //-32,768 to 32,767
        long bigNumber = 1; //-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        uint unsignedNumber = 1; //0 to 4,294,967,295
        ushort shortUnsignedNumber = 1; //0 to 65,535
        ulong bigUnsignedNumber = 1; //0 to 18,446,744,073,709,551,615
        float floatNumber = 1.99f; //-3.402823E+38 to 3.402823E+38
        double price = 1.99; //-1.7976931348623157E+308 to 1.7976931348623157E+308
        decimal amount = 1.99M; //-79228162514264337593543950335M to 79228162514264337593543950335M
        byte byteValue = 255; //0-255
        sbyte sbyteNumber = 1; //-128 to 127
        bool isWorking = true;
        char character = 'A';
        DateTime date = DateTime.Now;
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
        DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
        TimeOnly timeOnly = TimeOnly.FromDateTime(DateTime.Now);
        Guid guid = Guid.NewGuid();
        Point point = new Point(10, 20);

        // Reference types
        string name = "John";
        Person person = new Person { Name = "John", Age = 20 };
        object obj = new Person { Name = "John", Age = 20 };
        int[] array = new int[3] { 1, 2, 3 };
        List<int> list = new List<int> { 1, 2, 3 };
    }
}

[DebuggerDisplay("Name = {Name}, Age = {Age}")]
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}