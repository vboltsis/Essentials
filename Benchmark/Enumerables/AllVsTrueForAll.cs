namespace Benchmark;

/*
|        Method | Number |     Mean |    Error |   StdDev |   Gen0 | Allocated |
|-------------- |------- |---------:|---------:|---------:|-------:|----------:|
|       AllList |     10 | 81.65 ns | 1.668 ns | 1.638 ns | 0.0031 |      40 B |
| TrueForAllSet |     10 | 17.47 ns | 0.258 ns | 0.242 ns |      - |         - |
 */

[MemoryDiagnoser]
public class AllVsTrueForAll
{
    private List<int> _list;

    [Params(10)]
    public int Number;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _list = new List<int>(Enumerable.Range(1, Number));
    }

    [Benchmark]
    public bool AllList()
    {
        return _list.All(i => i > 0);
    }

    [Benchmark]
    public bool TrueForAllSet()
    {
        return _list.TrueForAll(i => i > 0);
    }
}
