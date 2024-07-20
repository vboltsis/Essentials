namespace Benchmark;

/*
| Method        | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Casting       |  2.237 ns | 0.0230 ns | 0.0204 ns |  1.00 |    0.00 |      - |         - |          NA |
| Materializing | 21.152 ns | 0.5754 ns | 1.6875 ns |  9.81 |    0.58 | 0.0057 |      48 B |          NA | 
*/

[MemoryDiagnoser]
public class CastingVsMaterializing
{
    private static IEnumerable<int> _list = new int[] { 1, 2, 3, 4, 5 };

    [Benchmark(Baseline = true)]
    public int Casting()
    {
        var array = _list as int[];
        var sum = 0;

        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }

        return sum;
    }

    [Benchmark]
    public int Materializing()
    {
        var array = _list.ToArray();
        var sum = 0;

        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }

        return sum;
    }
}
