using System.Text.RegularExpressions;

namespace Benchmark;

/*
|             Method |      Mean |    Error |   StdDev |   Gen0 | Allocated |
|------------------- |----------:|---------:|---------:|-------:|----------:|
|              Split |  37.49 ns | 0.451 ns | 0.377 ns | 0.0095 |     120 B |
| RegexSplitCompiled | 634.94 ns | 3.677 ns | 3.439 ns | 0.0916 |    1160 B |
*/

[MemoryDiagnoser]
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
        return Regex.Split(_text, "|vs|", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
