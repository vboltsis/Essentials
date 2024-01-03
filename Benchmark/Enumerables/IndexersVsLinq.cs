namespace Benchmark;
/*
| Method       | Mean      | Error     | StdDev    | Allocated |
|------------- |----------:|----------:|----------:|----------:|
| IndexerFirst | 0.2061 ns | 0.0024 ns | 0.0022 ns |         - |
| LinqFirst    | 7.1080 ns | 0.1580 ns | 0.1941 ns |         - |

| IndexerLast  | 0.2603 ns | 0.0234 ns | 0.0219 ns |         - |
| LinqLast     | 7.0455 ns | 0.1621 ns | 0.2709 ns |         - | 
*/
[MemoryDiagnoser]
public class IndexersVsLinq
{
    private static List<int> numbers = Enumerable.Range(0, 100_000).ToList();

    [Benchmark]
    public int IndexerFirst()
    {
        return numbers[0];
    }

    [Benchmark]
    public int LinqFirst()
    {
        return numbers.First();
    }

    [Benchmark]
    public int IndexerLast()
    {
        return numbers[^1];
    }

    [Benchmark]
    public int LinqLast()
    {
        return numbers.Last();
    }
}
