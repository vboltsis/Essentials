using System.Collections.Immutable;

namespace FeatureExamples;

/// <summary>
/// Highlights the new collection expression syntax introduced in C# 12.
/// </summary>
public static class CollectionExpressionExample
{
    public static void Run()
    {
        int[] numbers = [1, 2, 3];
        Console.WriteLine($"Numbers: {string.Join(", ", numbers)}");

        List<string> names = ["Alice", "Bob"];
        Console.WriteLine($"Names: {string.Join(", ", names)}");

        int[] combined = [..numbers, 4, 5];
        Console.WriteLine($"Combined: {string.Join(", ", combined)}");

        Span<int> span = [10, 20, 30];
        Console.WriteLine($"Span: {string.Join(", ", span.ToArray())}");

        ImmutableArray<string> tags = ["api", "search", "cache"];
        Console.WriteLine($"ImmutableArray: {string.Join(", ", tags)}");
    }
}

