namespace FeatureExamples;

public class InterningExample
{
    public static void Example()
    {
        string first = "hello";
        string second = "hello";
        Console.WriteLine(ReferenceEquals(first, second)); // True
        
        string dynamicString = new string(new char[] { 'h', 'e', 'l', 'l', 'o' });
        string internedString = string.Intern(dynamicString);

        Console.WriteLine(ReferenceEquals(dynamicString, internedString)); // False
        Console.WriteLine(ReferenceEquals("hello", internedString));       // True
    }
}