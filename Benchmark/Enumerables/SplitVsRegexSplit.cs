using System.Text.RegularExpressions;

namespace Benchmark;

/*
| Method             | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------------- |---------:|---------:|---------:|-------:|----------:|
| Split              | 43.50 ns | 0.853 ns | 0.838 ns | 0.0143 |     120 B |
| RegexSplitCompiled | 90.09 ns | 1.159 ns | 1.084 ns | 0.0248 |     208 B |
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class SplitVsRegexSplit
{
    private static string _text = "Takis |vs| Makis";

    [Benchmark]
    public string[] Split()
    {
        return _text.Contains("|VS|") ? _text.Split("|VS|") : _text.Split("|vs|");
    }

    [Benchmark]
    public string[] RegexSplitCompiled()
    {
        return Regex.Split(_text, @"\|vs\|", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
