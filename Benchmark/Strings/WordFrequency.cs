using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Benchmark;

/*
| Method                  | Mean      | Error     | StdDev    | Gen0      | Gen1      | Gen2     | Allocated  |
|------------------------ |----------:|----------:|----------:|----------:|----------:|---------:|-----------:|
| ReturnWordFrequencySlow | 12.635 ms | 0.2419 ms | 0.2484 ms | 1296.8750 | 1250.0000 | 578.1250 | 10720.7 KB |
| ReturnWordFrequencyFast |  2.970 ms | 0.0096 ms | 0.0089 ms |   27.3438 |   27.3438 |  27.3438 |  311.17 KB | 
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class WordFrequency
{
    public static string Text = File
        .ReadAllText("C:\\Users\\Evangelos\\Desktop\\Code\\Essentials\\Preview\\TheSantaClaus.txt");

    [Benchmark]
    public Dictionary<string, int> ReturnWordFrequencySlow()
    {
        Dictionary<string, int> frequency = [];

        for (int i = 0; i < 2; i++)
        {
            foreach (Match match in Helper.Words().Matches(Text))
            {
                string word = match.Value;

                frequency[word] = frequency.TryGetValue(word, out int count) ? count + 1 : 1;
            }
        }

        return frequency;
    }

    [Benchmark]
    public Dictionary<string, int> ReturnWordFrequencyFast()
    {
        Dictionary<string, int> frequency = [];
        var lookup = frequency.GetAlternateLookup<ReadOnlySpan<char>>();

        for (int i = 0; i < 2; i++)
        {
            foreach (var match in Helper.Words().EnumerateMatches(Text))
            {
                var word = Text.AsSpan(match.Index, match.Length);
                CollectionsMarshal.GetValueRefOrAddDefault(lookup, word, out _)++;
            }
        }

        return frequency;
    }
}

public partial class Helper
{
    [GeneratedRegex(@"\b\w+\b")]
    public static partial Regex Words();
}