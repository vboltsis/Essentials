namespace FeatureExamples;

internal class OverloadExample
{
    public static void Print()
    {
        Console.WriteLine("OverloadExample");
    }

    public static void Print(string message)
    {
        Console.WriteLine(message);
    }

    public static void Print(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
