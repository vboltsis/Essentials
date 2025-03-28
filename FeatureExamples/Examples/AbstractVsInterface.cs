namespace FeatureExamples;

//Abstract classes exist to be inherited from, but cannot be instantiated.
public abstract class AbstractVsInterface
{
}

public interface IHuman
{
    public string FirstName { get; set; }

    public void Print()
    {
        Console.WriteLine("Hello world");
    }
}

class People
{
    public string FirstName { get; init; }

    public People(string firstName)
    {
        FirstName = firstName;
    }

    void PrintName()
    {
        Console.WriteLine(FirstName);
    }

    void Test()
    {
        var teacher = new Teacher("John", "Doe");

        var teacher2 = teacher with { FirstName = "Takis" };

        //teacher.FirstName = "Takis"; //cannot compile
    }

    void TestKitten()
    {
        var kitty = new Kitten("Spooky", 3);
        var kitty2 = new Kitten("Son", 4);
        var kitty3 = new Kitten("Son", 4);

        var set = new HashSet<Kitten>
        {
            kitty,
            kitty2,
            kitty3
        };
    }
}

public record Teacher(string FirstName, string LastName);

public class Teacher2
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public Teacher2(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public record Kitten(string Name, int Age);

//[DebuggerDisplay("Name = {Name}, Age = {Age}")]
//public class Kitten
//{
//    public Kitten(string name, int age)
//    {
//        Name = name;
//        Age = age;
//    }

//    public string Name { get; set; }
//    public int Age { get; set; }
//}