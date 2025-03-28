using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FeatureExamples;

public class CollectionsMarshalExamples
{
    public static void InternalArrayOfList()
    {
        List<int> numbers = new(5) { 1, 2, 3, 4, 5 };

        // Access the internal array as a Span
        Span<int> span = CollectionsMarshal.AsSpan(numbers);

        // Modify the span, which directly modifies the list
        for (int i = 0; i < span.Length; i++)
        {
            span[i] *= 2;
        }

        Console.WriteLine(string.Join(", ", numbers)); // Output: 2, 4, 6, 8, 10
    }

    public static void GetReferenceFromDictionary()
    {
        Dictionary<string, int> wordCounts = new();

        // Add a word or get its reference
        ref int count = ref CollectionsMarshal.GetValueRefOrAddDefault(wordCounts, "example", out bool exists);

        // Update the value directly
        count++;
        Console.WriteLine($"'example' count: {wordCounts["example"]}"); // Output: 'example' count: 1

    }

    public static void GetValueOrNullFromDictionary()
    {
        Dictionary<string, int> wordCounts = new()
        {
            { "example", 1 }
        };

        // Access the value by reference
        ref int count = ref CollectionsMarshal.GetValueRefOrNullRef(wordCounts, "example");

        if (!Unsafe.IsNullRef(ref count))
        {
            count++;
            Console.WriteLine($"'example' count: {wordCounts["example"]}"); // Output: 'example' count: 2
        }
    }
}
