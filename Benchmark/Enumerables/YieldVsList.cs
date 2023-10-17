namespace Benchmark;

/*
|       Method |     Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|------------- |---------:|----------:|----------:|-------:|-------:|----------:|
|  ConsumeList | 6.108 us | 0.1195 us | 0.2214 us | 0.4883 | 0.0076 |    4096 B |
| ConsumeYield | 3.156 us | 0.0626 us | 0.1097 us | 0.0038 |      - |      32 B |
|      SumList | 1.085 us | 0.0064 us | 0.0053 us | 0.4845 |      - |    4056 B |
|     SumYield | 3.481 us | 0.0400 us | 0.0374 us | 0.0038 |      - |      32 B |
*/

[RankColumn]
[MemoryDiagnoser]
public class YieldVsList
{
    [Benchmark(Baseline = true)]
    public void ConsumeList()
    {
        var list = GetList();

        foreach (var item in list)
        {
            var a = item;
        }
    }

    [Benchmark]
    public void ConsumeYield()
    {
        var list = GetYield();

        foreach (var item in list)
        {
            var a = item;
        }
    }

    [Benchmark]
    public int SumList()
    {
        return GetList().Sum();
    }

    [Benchmark]
    public int SumYield()
    {
        return GetYield().Sum();
    }

    public IEnumerable<int> GetList()
    {
        var list = new List<int>(1000);
        for (int i = 0; i < 1000; i++)
        {
            list.Add(i);
        }
        return list;
    }

    public IEnumerable<int> GetYield()
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return i;
        }
    }
}
