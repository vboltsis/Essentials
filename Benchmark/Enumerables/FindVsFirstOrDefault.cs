namespace Benchmark;

/*
|             Method | Number |      Mean |     Error |    StdDev | Rank |   Gen0 | Allocated |
|------------------- |------- |----------:|----------:|----------:|-----:|-------:|----------:|
|           FindList |     10 |  9.250 ns | 0.0194 ns | 0.0172 ns |    1 |      - |         - |
| FirstOrDefaultList |     10 | 52.044 ns | 0.9413 ns | 0.7860 ns |    3 | 0.0048 |      40 B |
|           FindCars |     10 | 13.931 ns | 0.0463 ns | 0.0433 ns |    2 |      - |         - |
| FirstOrDefaultCars |     10 | 71.897 ns | 1.1182 ns | 0.9338 ns |    4 | 0.0048 |      40 B |
 */

[MemoryDiagnoser]
[RankColumn]
public class FindVsFirstOrDefault
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
    public int FindList()
    {
        return _list.Find(i => i > 5);
    }

    [Benchmark]
    public int FirstOrDefaultList()
    {
        return _list.FirstOrDefault(i => i > 5);
    }

    [Benchmark]
    public Car FindCars()
    {
        return _cars.Find(c => c.Year > 2005);
    }

    [Benchmark]
    public Car FirstOrDefaultCars()
    {
        return _cars.FirstOrDefault(c => c.Year > 2005);
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

public class Car
{
    public string Name { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
}