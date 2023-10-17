using BenchmarkDotNet.Attributes;

namespace Benchmark;

/*
 |    Method |                 Text |      Mean |     Error |    StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|---------- |--------------------- |----------:|----------:|----------:|------:|-------:|----------:|------------:|
| SpanSlice |                   11 |  8.260 ns | 0.0733 ns | 0.0650 ns |  1.00 |      - |         - |          NA |
| SubString |                   11 |  9.661 ns | 0.0432 ns | 0.0383 ns |  1.17 |      - |         - |          NA |
|           |                      |           |           |           |       |        |           |             |
| SpanSlice | 22Tas(...)vjsdn [23] |  8.061 ns | 0.0672 ns | 0.0561 ns |  1.00 |      - |         - |          NA |
| SubString | 22Tas(...)vjsdn [23] | 13.543 ns | 0.0370 ns | 0.0328 ns |  1.68 | 0.0038 |      32 B |          NA |
 */

[MemoryDiagnoser]
public class SpanVsSubstring
{
    [Params("11", "22Tasrwuegfudvfjybvjsdn")]
    public string Text { get; set; }

    [Benchmark(Baseline = true)]
    public int SpanSlice()
    {
        var span = Text.AsSpan();
        var number = int.Parse(span[..2]);

        return number;
    }

    [Benchmark]
    public int SubString()
    {
        var number = int.Parse(Text[..2]);

        return number;
    }

    //[Benchmark]
    //public bool SliceEquals()
    //{
    //    var one = "1";
    //    var text = "1jklj";
    //    var span = text.AsSpan().Slice(0, 1);
    //    return MemoryExtensions.Equals(one, span, StringComparison.Ordinal);
    //}

    //[Benchmark]
    //public bool SubEquals()
    //{
    //    var one = "1";
    //    var text = "1jklj";
    //    return text.Substring(0, 1) == one;
    //}

    //[Params("11text11, 111text111")]
    //public string Target { get; set; }

    //[Benchmark]
    //[Arguments("1")]
    //public string TrimStartSpan(string trimString)
    //{
    //    ReadOnlySpan<char> result = Target.AsSpan();
    //    while (result.StartsWith(trimString))
    //    {
    //        result = result.Slice(trimString.Length);
    //    }

    //    return result.ToString();
    //}

    //[Benchmark]
    //[Arguments("1")]
    //public string TrimStart(string trimString)
    //{
    //    string result = Target;
    //    while (result.StartsWith(trimString))
    //    {
    //        result = result.Substring(trimString.Length);
    //    }

    //    return result;
    //}

    //[Benchmark]
    //[Arguments("1")]
    //public string TrimEndSpan(string trimString)
    //{
    //    ReadOnlySpan<char> result = Target.AsSpan();
    //    while (result.EndsWith(trimString))
    //    {
    //        result = result.Slice(0, result.Length - trimString.Length);
    //    }

    //    return result.ToString();
    //}

    //[Benchmark]
    //[Arguments("1")]
    //public string TrimEnd(string trimString)
    //{
    //    string result = Target;
    //    while (result.EndsWith(trimString))
    //    {
    //        result = result.Substring(0, result.Length - trimString.Length);
    //    }

    //    return result;
    //}

    //[Benchmark]
    //[Arguments("1")]
    //public string Replace(string trimString)
    //{
    //    string result = Target;
    //    return result.Replace(trimString, "");
    //}
}
