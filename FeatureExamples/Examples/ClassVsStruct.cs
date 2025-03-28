namespace FeatureExamples;

//a static class contains only static members
static class StaticClass
{
    static void Print()
    {
        Console.WriteLine("test");
    }

    static HashSet<int> numbers = new HashSet<int>
    {
        1,2,3,4,5
    };
}

//Non static classes can contain static members and non static members
//Static methods can be accessed without creating an instance of the class
public class Examples
{
    //Static ctors are called before any static members are accessed
    //Static ctors are called only once
    //Static ctors cannot be called directly
    //Static ctors cannot have access modifiers
    //Static ctors cannot have parameters
    static Examples()
    {
        Console.WriteLine("This is the ctor");
    }

    public static void StaticTest()
    {
        Console.WriteLine("hello");
    }

    static List<int> list = new List<int>();

    public string Name { get; set; }

    public void Test()
    {
        Human human = default;
        DateTime date = default;
        HumanStruct humanStruct = new HumanStruct
        {
            Age = 1,
            FirstName = "test",
            LastName = "test"
        };

        HumanStruct humanStruct2 = new HumanStruct
        {
            Age = 1,
            FirstName = "test",
            LastName = "test"
        };

        //Console.WriteLine(humanStruct == humanStruct2); //won't compile for structs. Will compile for record structs

        Console.WriteLine("test");
    }
}

class Human
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

//one class can inherit from ONE class
class Student : Human
{
    public int Age { get; set; }
}

//structs cannot inherit from other structs
struct HumanStruct
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

readonly record struct HumanStructReadOnly
{
    public readonly string FirstName;
    public readonly string LastName;
    public readonly int Age;

    public HumanStructReadOnly(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }
}