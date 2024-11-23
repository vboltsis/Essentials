using System.Runtime.CompilerServices;

namespace Preview;

public static class MathHelper
{
    [OverloadResolutionPriority(1)] // This overload will have a lower priority
    public static void Calculate(double number = 20)
    {
        Console.WriteLine("Double overload called with value: " + number);
    }

    [OverloadResolutionPriority(2)] // This overload will be favored when both match
    public static void Calculate(int number = 10)
    {
        Console.WriteLine("Integer overload called with value: " + number);
    }
}

public class Formatter
{
    [OverloadResolutionPriority(1)]
    public void Format(string text) => Console.WriteLine("string overload called");

    [OverloadResolutionPriority(2)]
    public void Format(ReadOnlySpan<char> chars) => Console.WriteLine("Span overload called");
}
