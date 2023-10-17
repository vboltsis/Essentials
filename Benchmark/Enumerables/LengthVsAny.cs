namespace Benchmark;

/*
| Method | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
|------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|----------:|------------:|
| Length | 0.0058 ns | 0.0099 ns | 0.0083 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |
| Any    | 6.3964 ns | 0.1525 ns | 0.1498 ns | 6.3528 ns |     ? |       ? |    2 |         - |           ? |
 */

[RankColumn]
[MemoryDiagnoser]
public class LengthVsAny
{
    private const int N = 1000;

    private readonly int[] _array = new int[N];

    [Benchmark(Baseline = true)]
    public bool Length()
    {
        return _array.Length > 0;
    }

    [Benchmark]
    public bool Any()
    {
        return _array.Any();
    }
}
