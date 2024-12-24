using System.Text.RegularExpressions;

namespace Benchmark;

/*
|         Method |      Mean |    Error |   StdDev |   Gen0 | Allocated |
|--------------- |----------:|---------:|---------:|-------:|----------:|
|  UseSpanMethod |  14.75 ns | 0.042 ns | 0.038 ns |      - |         - |
| UseRegexMethod | 111.59 ns | 1.072 ns | 0.950 ns | 0.0331 |     416 B | 
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class NumericParseVsRegex
{
    private static readonly string Input = "abc123";
    private static readonly Regex RegexPattern = new Regex(@"[^\d]+(?<int>\d+)", RegexOptions.Compiled);

    [Benchmark]
    public int? UseSpanMethod()
    {
        return FindFirstIntAfterNonDigits(Input);
    }

    [Benchmark]
    public int UseRegexMethod()
    {
        var match = RegexPattern.Match(Input);
        return int.Parse(match.Groups["int"].Value);
    }

    private int? FindFirstIntAfterNonDigits(string input)
    {
        ReadOnlySpan<char> span = input.AsSpan();
        bool foundNonDigit = false;

        for (int i = 0; i < span.Length; i++)
        {
            if (!char.IsDigit(span[i]))
            {
                foundNonDigit = true;
            }
            else if (foundNonDigit)
            {
                int start = i;

                while (i < span.Length && char.IsDigit(span[i]))
                {
                    i++;
                }

                if (int.TryParse(span.Slice(start, i - start), out int result))
                {
                    return result;
                }
            }
        }

        return null;
    }
}