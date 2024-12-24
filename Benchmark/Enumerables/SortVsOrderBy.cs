namespace Benchmark;

/*
|       Method |      Mean |    Error |   StdDev |   Gen0 | Allocated |
|------------- |----------:|---------:|---------:|-------:|----------:|
|     SortList |  50.70 ns | 0.526 ns | 0.466 ns |      - |         - |
|  OrderByList | 155.99 ns | 1.642 ns | 1.282 ns | 0.0432 |     544 B |
|    SortArray |  48.19 ns | 0.857 ns | 0.715 ns |      - |         - |
| OrderByArray | 170.29 ns | 3.333 ns | 4.334 ns | 0.0408 |     512 B |
 */

[MemoryDiagnoser] 
public class SortVsOrderBy
{
    private readonly List<Comparable> _list = new List<Comparable>(10);
    private readonly Comparable[] _array = new Comparable[10];

    [GlobalSetup]
    public void GlobalSetup()
    {
        for (int i = 0; i < 10; i++)
        {
            _list.Add(new Comparable { Name = $"Takis{i}", Age = i });
            _array[i] = new Comparable { Name = $"Takis{i}", Age = i };
        }
    }

    [Benchmark]
    public List<Comparable> SortList()
    {
        _list.Sort((a, b) => a.Age.CompareTo(b.Age));
        return _list;
    }

    [Benchmark]
    public List<Comparable> OrderByList()
    {
        var ordered = _list.OrderBy(x => x.Age);

        return ordered.ToList();
    }

    [Benchmark]
    public Comparable[] SortArray()
    {
        Array.Sort(_array, (a, b) => a.Age.CompareTo(b.Age));

        return _array;
    }

    [Benchmark]
    public Comparable[] OrderByArray()
    {
        var ordered = _array.OrderBy(x => x.Age);

        return ordered.ToArray();
    }
}

public class Comparable
{
    public string Name { get; set; }
    public int Age { get; set; }
}