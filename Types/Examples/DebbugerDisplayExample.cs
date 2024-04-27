using System.Diagnostics;

namespace FeatureExamples;

[DebuggerDisplay("Age: {Age}, Name: {Name}, Address: {Address}")]
class TestDisplay
{
    public int Age { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public static void Test()
    {
        var test = new TestDisplay
        {
            Age = 2,
            Name = "Takis",
            Address = "Test"
        };
        
        Console.WriteLine("Hello");
    }
}