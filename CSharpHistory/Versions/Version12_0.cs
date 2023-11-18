using Point = (int x, int y);

namespace CSharpHistory.Versions;

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
}

//PRIMARY CONSTRUCTOR
public class Cat(string name)
{
    public string Name { get; } = name;
}

public struct Dog(string name)
{
    public string Name { get; set; } = name;
}