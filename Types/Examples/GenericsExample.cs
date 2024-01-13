using System.Drawing;

namespace FeatureExamples;

//Generic classes and methods combine reusability, type safety,
//and efficiency in a way that their non-generic counterparts cannot.

internal class GenericsExample<T> where T : struct
{
    private T[] _array;

    public GenericsExample(T[] array)
    {
        _array = array;
    }

    public void PrintArray()
    {
        foreach (var item in _array)
        {
            Console.WriteLine(item);
        }
    }
}

public class ExampleRunner
{
    public static T PrintClassProperties<T>(T obj)
    {
        foreach (var property in obj.GetType().GetProperties())
        {
            Console.WriteLine(property.Name + ": " + property.GetValue(obj));
        }

        return obj;
    }

    public static void RunExample()
    {
        var array = new Test[] 
        {
            new Test
            {
                Address = "123 Main St",
                Age = 30,
                City = "New York",
                Name = "John"
            },
            new Test
            {
                Address = "456 Main St",
                Age = 40,
                City = "New York",
                Name = "Jane"
            }
        };

        var generic = new GenericsExample<Test>(array);
        generic.PrintArray();

        var test = new Something
        {
            Color = Color.Red,
            Day = DayOfWeek.Monday,
            Age = 30,
            Name = "John"
        };

        PrintClassProperties(test);
    }
}

file record struct Test
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
}

file class Something
{
    public Color Color { get; set; }
    public DayOfWeek Day { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
}
