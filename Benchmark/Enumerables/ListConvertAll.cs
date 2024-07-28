namespace Benchmark;

/*
| Method     | Mean     | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|----------- |---------:|----------:|----------:|-------:|-------:|----------:|
| ConvertAll | 7.957 us | 0.1588 us | 0.2610 us | 3.6316 | 0.3967 |  29.77 KB |
| Select     | 6.706 us | 0.1328 us | 0.3589 us | 3.6469 | 0.4044 |  29.84 KB |
 */

[MemoryDiagnoser]
public class ListConvertAll
{
    private List<int> _list;

    [GlobalSetup]
    public void Setup()
    {
        _list = new List<int>(Enumerable.Range(1, 1000));
    }

    [Benchmark]
    public List<string> ConvertAll()
    {
        return _list.ConvertAll(i => i.ToString());
    }

    [Benchmark]
    public List<string> Select()
    {
        return _list.Select(i => i.ToString()).ToList();
    }
}
