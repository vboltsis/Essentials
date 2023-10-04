namespace FeatureExamples;

public class StaticConstructor
{
    public static string _name = "makis";

    //it is called only once
    //it is called before the first instance is created
    //Static constructors must be parameterless
    static StaticConstructor()
    {
        Console.WriteLine("HELLO FROM STATIC");
    }

    public StaticConstructor()
    {
        Console.WriteLine("Hello from non static");
    }

    public StaticConstructor(string text)
    {
        Console.WriteLine(text);
    }
}

class Takis
{

}