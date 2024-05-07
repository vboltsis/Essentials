namespace CSharpFundamentals;

public class Methods
{
    /// <summary>
    /// Prints the text to the console
    /// </summary>
    /// <param name="text">The text to print</param>
    public void Print(string text)
    {
        Console.WriteLine(text);
    }

    /// <summary>
    /// Static way to print the text to the console
    /// </summary>
    /// <param name="text">The text to print</param>
    public static void PrintStatic(string text)
    {
        Console.WriteLine(text);
    }

    public static void DefaultValues(int number, string text = "Hello")
    {
        Console.WriteLine($"Number: {number}, Text: {text}");
    }
}
