using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Point = (int x, int y);

namespace CSharpHistory;

internal class Version12_0
{
    public void Method()
    {
        var kitten = new Cat("Spooky");
        Console.WriteLine(kitten.Name);

        var doggo = new Dog("Barky");
        Console.WriteLine(doggo.Name);

        //COLLECTION EXPRESSIONS
        Cat[] array = [new Cat("Snow"), new Cat("Blacky")];
        List<Cat> list = [new Cat("Snow"), new Cat("Blacky")];

        int[] nums = [1, 2, 3];
        Span<int> span = [4, 5, 6];

        int[] merge = [.. nums, .. span]; // [1, 2, 3, 4, 5, 6]
        //ALIAS ANY TYPE
        var point = new Point(1, 2);

        Console.WriteLine(point.y); // 2

        //OPTIONAL LAMBDA EXPRESSION PARAMETERS
        var addWithDefault = (int addTo = 2) => addTo + 1;
        addWithDefault(); // 3
        addWithDefault(5); // 6

        var numbers = (params int[] xs) => xs.Length;
        numbers(); // 0
        numbers(1, 2, 3); // 3
    }

    public void Method2()
    {
        var numbers = new int[] { 1, 2, 6 };

        var span = new Span<int>([9, 4, 5]);

        var combined = numbers.Concat(span.ToArray()).ToArray();

        var list = new List<Cat> { new("John"), new("Snow") };
    }
}

//PRIMARY CONSTRUCTOR
public class Cat(string name)
{
    public string Name { get; } = name;

    public void Meow()
    {
        Console.WriteLine($"{Name} shouts: Meow");
    }
}

public struct Dog(string name)
{
    public string Name { get; set; } = name;

    public void Bark()
    {
        Console.WriteLine($"{Name} shouts: Bark");
    }

}

[InlineArray(10)]
public struct InlineBuffer
{
    private int _element0;
}

[Experimental("WillChange")]
public class TestClass
{
}

//public static class Interceptor
//{
//    [InterceptsLocation(
//        filePath: "path",
//        line: 120,
//        character: 300
//        )]
//    public static void InterceptMethod1(
//        this Example example)
//    {
//        Console.WriteLine("Intercepted Method1");
//    }
//}

//public class Example
//{
//    public void Method1()
//    {
//        Console.WriteLine("Method1");
//    }
//}

//class RefReadonlyExample
//{
//    public void Method(in int number)
//    {
//        number++; // won't compile because it is a readonly variable
//    }

//    public void Method2(ref readonly int number)
//    {
//        number++; // will compile because it is a reference variable
//    }
//}