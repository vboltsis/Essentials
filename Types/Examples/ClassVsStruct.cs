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
    public static void StaticTest()
    {
        Console.WriteLine("hello");
    }

    static List<int> list = new List<int>();

    public void Test()
    {
        Human human = default;
        HumanStruct humanStruct = default;
        DateTime date = default;

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