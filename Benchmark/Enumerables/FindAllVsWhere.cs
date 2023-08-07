namespace Benchmark;

/*
|         Method | Number |     Mean |    Error |   StdDev | Rank |   Gen0 | Allocated |
|--------------- |------- |---------:|---------:|---------:|-----:|-------:|----------:|
|    FindAllList |     10 | 41.18 ns | 0.855 ns | 1.081 ns |    1 | 0.0153 |     128 B |
|      WhereList |     10 | 65.56 ns | 1.310 ns | 1.287 ns |    3 | 0.0238 |     200 B |
|     WhereArray |     10 | 71.12 ns | 1.143 ns | 1.013 ns |    5 | 0.0257 |     216 B |

|    FindAllCars |     10 | 46.86 ns | 0.973 ns | 1.729 ns |    2 | 0.0105 |      88 B |
|      WhereCars |     10 | 68.72 ns | 0.818 ns | 0.725 ns |    4 | 0.0191 |     160 B |
| WhereCarsArray |     10 | 75.95 ns | 0.454 ns | 0.403 ns |    6 | 0.0153 |     128 B |
 */

[MemoryDiagnoser]
[RankColumn]
public class FindAllVsWhere
{
    private List<int> _list;
    private List<Car> _cars;

    [Params(10)]
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
