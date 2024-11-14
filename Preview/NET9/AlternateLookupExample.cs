using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Preview;

public class AlternateLookupExample
{
    public static Dictionary<string, int> ReturnWordFrequency()
    {
        var text = File.ReadAllText("path to text");

        Dictionary<string, int> frequency = [];
        var lookup = frequency.GetAlternateLookup<ReadOnlySpan<char>>();

        for (int i = 0; i < 10; i++)
        {
            foreach (var match in Helper.Words().EnumerateMatches(text))
            {
                var word = text.AsSpan(match.Index, match.Length);
                CollectionsMarshal.GetValueRefOrAddDefault(lookup, word, out _)++;
            }
        }

        return frequency;
    }

    public static Dictionary<string,int> ReturnWordFrequencyWithMetrics()
    {
        var text = File
            .ReadAllText("C:\\Users\\Evangelos\\Desktop\\Code\\Essentials\\Preview\\TheSantaClaus.txt");

        var time = Stopwatch.GetTimestamp();

        Dictionary<string, int> frequency = [];
        var lookup = frequency.GetAlternateLookup<ReadOnlySpan<char>>();

        for (int i = 0; i < 10; i++)
        {
            var memory = GC.GetTotalAllocatedBytes();
            foreach (var match in Helper.Words().EnumerateMatches(text))
            {
                var word = text.AsSpan(match.Index, match.Length);
                CollectionsMarshal.GetValueRefOrAddDefault(lookup, word, out _)++;
            }
            Console.WriteLine($"Allocated Memory: {memory / 1024.0 / 1024:N2}MB");
        }

        var elapsed = Stopwatch.GetElapsedTime(time);

        Console.WriteLine(elapsed.Milliseconds);

        return frequency;
    }
}

public partial class Helper
{
    [GeneratedRegex(@"\b\w+\b")]
    public static partial Regex Words();
}