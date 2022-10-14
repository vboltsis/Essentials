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
}
