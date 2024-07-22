using System.Buffers;

namespace FeatureExamples;

public class VowelChecker
{
    private static readonly char[] Vowels = ['a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U'];
    private static readonly SearchValues<char> vowelSearchValues = SearchValues.Create(Vowels);

    public static bool AreAllCharactersVowels(string input)
    {
        return input.AsSpan().IndexOfAnyExcept(vowelSearchValues) == -1;
    }

    public static void TestVowels()
    {
        var testInput = "aeiouAEIOU";
        var result = AreAllCharactersVowels(testInput); //true
        Console.WriteLine($"Are all characters in '{testInput}' vowels? {result}");

        testInput = "Hello World!";
        result = AreAllCharactersVowels(testInput); //false
        Console.WriteLine($"Are all characters in '{testInput}' vowels? {result}");
    }
}

public class HexValidator
{
    static readonly char[] HexCharacters = "0123456789abcdefABCDEF".ToCharArray();
    static readonly SearchValues<char> hexSearchValues = SearchValues.Create(HexCharacters);

    public static bool IsHexString(string input)
    {
        return input.AsSpan().IndexOfAnyExcept(hexSearchValues) == -1;
    }

    public static void TestHexValidator()
    {
        var testInput = "1A3F"; // Valid hex string
        var result = IsHexString(testInput); // true
        Console.WriteLine($"Is '{testInput}' a valid hex string? {result}");

        testInput = "1A3G"; // Invalid hex string (contains 'G')
        result = IsHexString(testInput); // false
        Console.WriteLine($"Is '{testInput}' a valid hex string? {result}");

        testInput = "1234567890abcdef"; // Valid hex string
        result = IsHexString(testInput); // true
        Console.WriteLine($"Is '{testInput}' a valid hex string? {result}");

        testInput = "1234567890ghijkl"; // Invalid hex string (contains 'g', 'h', 'i', 'j', 'k', 'l')
        result = IsHexString(testInput); // false
        Console.WriteLine($"Is '{testInput}' a valid hex string? {result}");
    }
}
