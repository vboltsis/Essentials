using System.Buffers;

namespace Benchmark;

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class SearchValuesString
{
    private static readonly string Haystack =
    new HttpClient().GetStringAsync("https://www.gutenberg.org/cache/epub/1661/pg1661.txt").Result;

    private static readonly string[] NeedleArray = ["Wriggled ", "Headstrong", "Wow"];

    private static readonly SearchValues<string> searchValues =
        SearchValues.Create(NeedleArray, StringComparison.OrdinalIgnoreCase);

    [Benchmark]
    public int IndexOfFirstArray()
    {
        ReadOnlySpan<char> haystack = Haystack;

        for (int i = 0; i < haystack.Length; i++)
        {
            foreach (var needle in NeedleArray)
            {
                if (haystack[i..].StartsWith(needle, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    [Benchmark]
    public int IndexOfFirstSearchValues()
    {
        ReadOnlySpan<char> haystack = Haystack;

        return haystack.IndexOfAny(searchValues);
    }
}
