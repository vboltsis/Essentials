namespace FeatureExamples;

public class VirtualExample
{
    public virtual void Print()
    {
        Console.WriteLine("VirtualExample");
    }

    public void Method()
    {
        Console.WriteLine("Method");
    }
}

public class VirtualExample2 : VirtualExample
{
    public override void Print()
    {
        base.Print();
        Console.WriteLine("VirtualExample2");
    }

    public new void Method()
    {
        Console.WriteLine("Method2");
    }
}

/*
Indexers allow objects to be indexed in a similar manner to arrays.
A virtual indexer can be overridden in a derived class.
*/
public class StringCollection
{
    protected string[] data = new string[10];
    public virtual string this[int index]
    {
        get { return data[index]; }
        set { data[index] = value; }
    }
}

public class UpperCaseStringCollection : StringCollection
{
    public override string this[int index]
    {
        get { return base[index]; }
        set { base[index] = value.ToUpper(); } // Override to store all strings in uppercase
    }

    public static void Example()
    {
        var normalCollection = new StringCollection();
        normalCollection[0] = "hello";
        normalCollection[1] = "world";

        var upperCaseCollection = new UpperCaseStringCollection();
        upperCaseCollection[0] = "hello";
        upperCaseCollection[1] = "world";

        Console.WriteLine("Normal Collection:");
        Console.WriteLine(normalCollection[0]); // Output: hello
        Console.WriteLine(normalCollection[1]); // Output: world

        Console.WriteLine("Upper Case Collection:");
        Console.WriteLine(upperCaseCollection[0]); // Output: HELLO
        Console.WriteLine(upperCaseCollection[1]); // Output: WORLD
    }
}
