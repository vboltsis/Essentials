var c1 = new C();

c1.Name = "test";

var c2 = new C();
c2.Name = "test";

Console.WriteLine(c1.Name);
Console.WriteLine(c1 == c2);
class Test
{
    public string Name { get; set; }
}

record Test2(string Name);

record struct Test3
{
    public int Number { get; init; }
}

public record C {
    public string Name { get; set; }
}

interface IInterface
{
    void Print()
    {
        Console.WriteLine("hello");
    }
}