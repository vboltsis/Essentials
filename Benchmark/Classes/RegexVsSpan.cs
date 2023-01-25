using System.Text.RegularExpressions;

namespace Benchmark.Classes;

/*
|                      Method |                Input |      Mean |    Error |   StdDev | Rank |   Gen0 | Allocated |
|---------------------------- |--------------------- |----------:|---------:|---------:|-----:|-------:|----------:|
|           ExtractWordsRegex | |maki(...)rest| [32] | 301.44 ns | 5.909 ns | 6.323 ns |    3 | 0.0725 |     912 B |
|          ExtractWordsManual | |maki(...)rest| [32] |  41.86 ns | 0.750 ns | 1.099 ns |    1 | 0.0147 |     184 B |
| ExtractWordsSourceGenerator | |maki(...)rest| [32] | 288.41 ns | 5.681 ns | 7.584 ns |    2 | 0.0725 |     912 B |
 */

[RankColumn]
[MemoryDiagnoser]
public partial class RegexVsSpan
{
    private const char _pipe = '|';

    [GeneratedRegex(@"\|(.*?)\|", RegexOptions.None, 100)]
    public static partial Regex _pipesMatch();

    private static readonly Regex _pipesMatchRegex = new Regex(@"\|(.*?)\|", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));


    [Params("|makis test| omg vs |takis rest|")]
    public string Input;

    [Benchmark]
    public string[] ExtractWordsRegex()
    {
        return _pipesMatchRegex.Matches(Input).Select(m => m.Value).ToArray();
    }

    [Benchmark]
    public string[] ExtractWordsManual()
    {
        var wordsWithin = new List<string>();

        var index = 0;
        while (index < Input.Length)
        {
            var start = Input.IndexOf(_pipe, index);

            if (start == -1)
                break;

            var end = Input.IndexOf(_pipe, start + 1);

            if (end == -1)
                break;

            wordsWithin.Add(Input.Substring(start + 1, end - start - 1));
            index = end + 1;
        }

        return wordsWithin.ToArray();
    }

    [Benchmark]
    public string[] ExtractWordsSourceGenerator()
    {
        return _pipesMatch().Matches(Input).Select(x => x.Value).ToArray();
    }
}

