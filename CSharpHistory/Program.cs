//var number = 0;

//for (var i = 0; i < 1000; i++)
//{
//    Task.Run(() =>
//    {
//        number++;
//    });
//}

//Console.WriteLine(number);

//var number = Fruit.apple;

//if (!(number is Fruit.orange or Fruit.apple))
//{
//    Console.WriteLine("hello");
//}

//Console.WriteLine();

//enum Fruit
//{
//    apple,
//    banana,
//    orange
//}

using System.Reflection;

var list = new List<string>();
var what = string.Join(",", list);

var person1 = new Person { Name = "John", Age = 20 };
var person2 = new Person { Name = "John", Age = 21 };

var propertiesChanged = CompareTo(person1, person2);

foreach (var property in propertiesChanged)
{
    Console.WriteLine(property);
}

Console.WriteLine();

static List<string> CompareTo<T>(T original, T compared)
{
    var propertiesChanged = new List<string>();
    var properties = typeof(T).GetProperties();
    foreach (PropertyInfo property in properties)
    {
        if (!property.PropertyType.IsPrimitive &&
            property.PropertyType != typeof(string) &&
            property.PropertyType != typeof(DateTime))
            continue;

        object originalValue = property.GetValue(original, null);
        object comparedValue = property.GetValue(compared, null);
        if (originalValue != null && comparedValue != null && !originalValue.Equals(comparedValue))
        {
            propertiesChanged.Add(property.Name);
        }
    }
    return propertiesChanged;
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}