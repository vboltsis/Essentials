using System.Text.RegularExpressions;

namespace Benchmark;

/*
|                      Method |                Input |      Mean |    Error |   StdDev | Rank |   Gen0 | Allocated |
|---------------------------- |--------------------- |----------:|---------:|---------:|-----:|-------:|----------:|
|           ExtractWordsRegex | |maki(...)rest| [32] | 245.27 ns | 4.365 ns | 4.083 ns |    2 | 0.0663 |     832 B |
|          ExtractWordsManual | |maki(...)rest| [32] |  49.93 ns | 1.009 ns | 1.571 ns |    1 | 0.0178 |     224 B |
| ExtractWordsSourceGenerator | |maki(...)rest| [32] | 248.26 ns | 4.877 ns | 6.167 ns |    2 | 0.0663 |     832 B |
 */

[RankColumn]
[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public partial class RegexVsManualVsSource
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

            wordsWithin.Add(Input.Substring(start, end - start + 1));
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
