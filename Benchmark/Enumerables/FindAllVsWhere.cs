namespace Benchmark;

/*
| Method         | Number | Mean        | Error      | StdDev     | Median      | Rank | Gen0   | Gen1   | Allocated |
|--------------- |------- |------------:|-----------:|-----------:|------------:|-----:|-------:|-------:|----------:|
| FindAllList    | 10     |    37.74 ns |   0.790 ns |   0.845 ns |    37.82 ns |    2 | 0.0153 |      - |     128 B |
| WhereList      | 10     |    50.92 ns |   0.144 ns |   0.135 ns |    50.90 ns |    3 | 0.0239 |      - |     200 B |
| WhereArray     | 10     |    58.47 ns |   1.217 ns |   2.647 ns |    57.12 ns |    5 | 0.0257 |      - |     216 B |

| FindAllCars    | 10     |    28.77 ns |   0.262 ns |   0.245 ns |    28.83 ns |    1 | 0.0105 |      - |      88 B |
| WhereCars      | 10     |    53.32 ns |   0.723 ns |   0.677 ns |    53.55 ns |    4 | 0.0191 |      - |     160 B |
| WhereCarsArray | 10     |    60.90 ns |   0.257 ns |   0.240 ns |    60.93 ns |    6 | 0.0153 |      - |     128 B |

| FindAllList    | 1000   | 2,217.96 ns |  38.305 ns |  35.831 ns | 2,231.41 ns |    7 | 1.0033 |      - |    8424 B |
| WhereList      | 1000   | 2,671.49 ns |  33.251 ns |  29.476 ns | 2,669.89 ns |    9 | 1.0147 | 0.0114 |    8496 B |
| WhereArray     | 1000   | 2,340.23 ns |  21.957 ns |  20.538 ns | 2,334.47 ns |    8 | 1.0109 |      - |    8456 B |

| FindAllCars    | 1000   | 4,490.84 ns |  89.093 ns | 151.286 ns | 4,430.61 ns |   11 | 1.9836 | 0.0534 |   16600 B |
| WhereCars      | 1000   | 4,316.85 ns |  59.746 ns |  46.645 ns | 4,327.10 ns |   10 | 1.9913 | 0.0534 |   16672 B |
| WhereCarsArray | 1000   | 5,259.60 ns | 103.787 ns | 244.639 ns | 5,206.34 ns |   12 | 1.9608 |      - |   16440 B |
 */

[MemoryDiagnoser]
[RankColumn]
public class FindAllVsWhere
{
    private List<int> _list;
    private List<Car> _cars;

    [Params(10, 1000)]
    public int Number;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _list = new List<int>(Enumerable.Range(1, Number));
        SetupCars();
    }

    [Benchmark]
    public List<int> FindAllList()
    {
        return _list.FindAll(i => i > 5);
    }

    [Benchmark]
    public List<int> WhereList()
    {
        return _list.Where(i => i > 5).ToList();
    }

    [Benchmark]
    public int[] WhereArray()
    {
        return _list.Where(i => i > 5).ToArray();
    }

    [Benchmark]
    public List<Car> FindAllCars()
    {
        return _cars.FindAll(c => c.Year > 2005);
    }

    [Benchmark]
    public List<Car> WhereCars()
    {
        return _cars.Where(c => c.Year > 2005).ToList();
    }

    [Benchmark]
    public Car[] WhereCarsArray()
    {
        return _cars.Where(c => c.Year > 2005).ToArray();
    }

    private void SetupCars()
    {
        _cars = new List<Car>(Number);
        for (int i = 0; i < Number; i++)
        {
            _cars.Add(new Car
            {
                Name = $"Car{i}",
                Year = 2000 + i,
                Month = 1 + i,
                Day = 1 + i
            });
        }
    }   
}
