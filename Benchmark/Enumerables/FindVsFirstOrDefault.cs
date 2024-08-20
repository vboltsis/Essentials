namespace Benchmark;

/*
| Method             | Number | Mean      | Error     | StdDev    | Rank | Gen0   | Allocated |
|------------------- |------- |----------:|----------:|----------:|-----:|-------:|----------:|
| FindList           | 10     |  3.960 ns | 0.0434 ns | 0.0362 ns |    1 |      - |         - |
| FirstOrDefaultList | 10     | 15.197 ns | 0.1253 ns | 0.1046 ns |    3 | 0.0032 |      40 B |
| FindCars           | 10     |  6.676 ns | 0.1416 ns | 0.2366 ns |    2 |      - |         - |
| FirstOrDefaultCars | 10     | 46.180 ns | 0.3461 ns | 0.2890 ns |    4 | 0.0032 |      40 B |
 */

[MemoryDiagnoser]
//[RankColumn]
public class FindVsFirstOrDefault
{
    private List<int> _list;
    //private List<Car> _cars;

    //[Params(10)]
    //public int Numbers;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _list = new List<int>(Enumerable.Range(1, 10));
        //SetupCars();
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

    //[Benchmark]
    //public Car FindCars()
    //{
    //    return _cars.Find(c => c.Year > 2005);
    //}

    //[Benchmark]
    //public Car FirstOrDefaultCars()
    //{
    //    return _cars.FirstOrDefault(c => c.Year > 2005);
    //}

    //private void SetupCars()
    //{
    //    _cars = new List<Car>(Number);
    //    for (int i = 0; i < Number; i++)
    //    {
    //        _cars.Add(new Car
    //        {
    //            Name = $"Car{i}",
    //            Year = 2000 + i,
    //            Month = 1 + i,
    //            Day = 1 + i
    //        });
    //    }
    //}
}

public class Car
{
    public string Name { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
}