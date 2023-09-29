namespace FeatureExamples;

public sealed class AbstractVsInterface
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

        //teacher.FirstName = "John"; //cannot compile
    }
}

public record Teacher(string FirstName, string LastName);