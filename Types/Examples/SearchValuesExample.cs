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