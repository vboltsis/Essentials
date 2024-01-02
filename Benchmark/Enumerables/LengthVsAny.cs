namespace Benchmark;

/*
| Method | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
|------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|----------:|------------:|
| Length | 0.0658 ns | 0.0257 ns | 0.0275 ns | 0.0494 ns |  1.00 |    0.00 |    1 |         - |          NA |
| Any    | 5.8000 ns | 0.0108 ns | 0.0101 ns | 5.8010 ns | 97.47 |   37.33 |    2 |         - |          NA |
 */

[RankColumn]
[MemoryDiagnoser]
public class LengthVsAny
{
    private readonly int[] _array = [1, 2, 3, 4];

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
