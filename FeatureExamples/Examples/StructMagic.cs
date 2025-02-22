using System.Runtime.InteropServices;

namespace FeatureExamples;

public class StructMagic
{
    public static void Example()
    {
        unsafe
        {
            Console.WriteLine(sizeof(Test1));
            Console.WriteLine(sizeof(Test2));
            Console.WriteLine(sizeof(Test3));
            Console.WriteLine(sizeof(Test4));
        }
    }
}

struct Test1
{
    byte b;
    double d;
    byte b2;
    double d2;
    byte b3;
}

struct Test2
{
    double d;
    double d2;
    byte b;
    byte b2;
    byte b3;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct Test3
{
    byte b;
    double d;
    byte b2;
    double d2;
    byte b3;
}

[StructLayout(LayoutKind.Explicit)]
struct Test4
{
    [FieldOffset(0)]
    byte b;
    [FieldOffset(8)]
    double d;
    [FieldOffset(16)]
    byte b2;
    [FieldOffset(24)]
    double d2;
    [FieldOffset(32)]
    byte b3;
}

//https://kalapos.net/Blog/ShowPost/DotNetConceptOfTheWeek13_DotNetMemoryLayout