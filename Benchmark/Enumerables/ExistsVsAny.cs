namespace Benchmark;

/*
| Method     | Number | Mean     | Error     | StdDev    | Gen0   | Allocated |
|----------- |------- |---------:|----------:|----------:|-------:|----------:|
| ExistsList | 10     | 1.259 ns | 0.0108 ns | 0.0084 ns |      - |         - |
| AnySet     | 10     | 8.501 ns | 0.1858 ns | 0.1988 ns | 0.0048 |      40 B |
*/

[MemoryDiagnoser]
public class ExistsVsAny
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
    public bool ExistsList()
    {
        return _list.Exists(i => i > 0);
    }

    [Benchmark]
    public bool AnySet()
    {
        return _list.Any(i => i > 0);
    }
}
