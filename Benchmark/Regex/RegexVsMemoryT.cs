using System.Text;
using System.Text.RegularExpressions;

namespace Benchmark;
/*
|              Method |                input |      Mean |     Error |    StdDev |    Median |   Gen0 | Allocated |
|-------------------- |--------------------- |----------:|----------:|----------:|----------:|-------:|----------:|
| TranslateSubstrings | This (...)orks. [69] | 164.32 ns |  3.307 ns |  4.299 ns | 161.96 ns | 0.0687 |     864 B |
|      TranslateRegex | This (...)orks. [69] | 666.18 ns | 12.536 ns | 12.312 ns | 663.96 ns | 0.1221 |    1536 B |

| TranslateSubstrings |               |Test| |  52.57 ns |  1.035 ns |  1.345 ns |  52.29 ns | 0.0191 |     240 B |
|      TranslateRegex |               |Test| | 375.36 ns |  6.578 ns |  7.575 ns | 376.10 ns | 0.0744 |     936 B |

| TranslateSubstrings |   |Test| |vs| |Test| | 126.78 ns |  1.930 ns |  1.612 ns | 127.10 ns | 0.0381 |     480 B |
|      TranslateRegex |   |Test| |vs| |Test| | 772.61 ns | 14.367 ns | 30.616 ns | 760.41 ns | 0.1564 |    1968 B | 
*/

[MemoryDiagnoser] 
public class RegexVsMemoryT
{
    private static Regex _pipesMatchRegex = new Regex(@"\|([^\|]+)\|", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

    [Params("This |test string| is designed to |demonstrate| how the |regex works.", "|Test|", "|Test| |vs| |Test|")]
    public string input;

    [Benchmark]
    public async Task<string> TranslateSubstrings()
    {
        var memory = input.AsMemory();
        var builder = new StringBuilder();

        while (true)
        {
            int start = memory.Span.IndexOf('|');
            if (start == -1)
            {
                builder.Append(memory);
                break;
            }
            builder.Append(memory.Slice(0, start).ToString());
            memory = memory.Slice(start + 1);
            int end = memory.Span.IndexOf('|');
            if (end == -1)
            {
                builder.Append("|" + memory.ToString());
                break;
            }
            string toTranslate = memory.Slice(0, end).ToString();
            string translation = "Takis";
            builder.Append(translation);
            memory = memory.Slice(end + 1);
        }

        return builder.ToString();
    }

    [Benchmark]
    public async Task<string> TranslateSubstrings2()
    {
        var memory = input.AsMemory();
        var builder = new StringBuilder();

        while (true)
        {
            int start = memory.Span.IndexOf('|');
            if (start == -1)
            {
                builder.Append(memory);
                break;
            }
            builder.Append(memory.Slice(0, start));
            memory = memory.Slice(start + 1);
            int end = memory.Span.IndexOf('|');
            if (end == -1)
            {
                builder.Append($"|{memory}");
                break;
            }
            string toTranslate = memory.Slice(0, end).ToString();
            string translation = "Takis";
            builder.Append(translation);
            memory = memory.Slice(end + 1);
        }

        return builder.ToString();
    }

    [Benchmark]
    public async Task<string> TranslateRegex()
    {
        var matches = _pipesMatchRegex.Matches(input);
        var translations = new List<string>(matches.Count);

        foreach (var item in matches)
        {
            translations.Add("Takis");
        }

        var i = 0;
        return Regex.Replace(input, _pipesMatchRegex.ToString(), m => translations[i++]);
    }
}
