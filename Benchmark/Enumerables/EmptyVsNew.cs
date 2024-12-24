using BenchmarkDotNet.Engines;

namespace Benchmark;

/*
|          Method |      Mean |     Error |    StdDev |    Median |   Gen0 | Allocated |
|---------------- |----------:|----------:|----------:|----------:|-------:|----------:|
|           Empty | 0.6805 ns | 0.0399 ns | 0.0354 ns | 0.6798 ns |      - |         - |
|             New | 3.1648 ns | 0.0808 ns | 0.0755 ns | 3.1766 ns | 0.0019 |      24 B |
|       EmptyList | 4.3607 ns | 0.1082 ns | 0.1157 ns | 4.3609 ns | 0.0025 |      32 B |
|         NewList | 4.2398 ns | 0.0955 ns | 0.0894 ns | 4.2153 ns | 0.0025 |      32 B |
| EmptyEnumerable | 0.0135 ns | 0.0160 ns | 0.0150 ns | 0.0084 ns |      - |         - |
 */

[MemoryDiagnoser] 
public class EmptyVsNew
{
    [Benchmark]
    public int[] Empty()
    {
        var array = Array.Empty<int>();

        return array;
    }

    [Benchmark]
    public int[] New()
    {
        var array = new int[0];

        return array;
    }

    [Benchmark]
    public List<int> EmptyList()
    {
        var list = Enumerable.Empty<int>().ToList();

        return list;
    }

    [Benchmark]
    public List<int> NewList()
    {
        var list = new List<int>(0);

        return list;
    }

    [Benchmark]
    public int EmptyEnumerable()
    {
        var enumerable = Enumerable.Empty<int>();

        return 1;
    }
}
