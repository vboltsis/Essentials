using System.Text;
using System.Text.RegularExpressions;

namespace Benchmark;

/*
|        Method |                Input |      Mean |    Error |   StdDev | Rank |   Gen0 | Allocated |
|-------------- |--------------------- |----------:|---------:|---------:|-----:|-------:|----------:|
|  ReplaceRegex | |maki(...)rest| [32] | 285.25 ns | 2.759 ns | 2.154 ns |    2 | 0.0515 |     648 B |
| ReplaceManual | |maki(...)rest| [32] |  52.97 ns | 0.268 ns | 0.209 ns |    1 | 0.0121 |     152 B |
 */

[RankColumn]
[MemoryDiagnoser]
public class ReplaceRegexVsManual
{
    private const string Pattern = @"\|([^\|]+)\|";
    private static string[] Replacements = new string[]
    {
        "test 1",
        "test 2"
    };

    private static string[] Codes = new string[]
    {
        "makis test",
        "takis rest"
    };

    [Params("|makis test| omg vs |takis rest|")]
    public string Input;

    [Benchmark]
    public string ReplaceRegex()
    {
        int k = 0;
        var result = Regex.Replace(Input, Pattern, m => Replacements[k++]);

        return result;
    }

    [Benchmark]
    public string ReplaceManual()
    {
        var result = Input;
        for (int i = 0; i < Codes.Length; i++)
        {
            var code = Codes[i];
            var replacement = Replacements[i];

            result = result.Replace(code, replacement);
        }

        return result;
    }

    [Benchmark]
    public string ReplaceBuilderManual()
    {
        var result = new StringBuilder(Input);

        for (int i = 0; i < Codes.Length; i++)
        {
            var code = Codes[i];
            var replacement = Replacements[i];

            result.Replace(code, replacement);
        }

        return result.ToString();
    }
}
