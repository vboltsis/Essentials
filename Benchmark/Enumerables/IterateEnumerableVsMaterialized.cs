namespace Benchmark.Classes;

/*
|                           Method |     Mean |   Error |  StdDev | Rank |   Gen0 | Allocated |
|--------------------------------- |---------:|--------:|--------:|-----:|-------:|----------:|
|          GetAndIterateEnumerable | 107.7 ns | 2.15 ns | 3.60 ns |    2 | 0.0147 |     184 B |
|  MaterializeAndIterateEnumerable | 101.9 ns | 1.22 ns | 1.08 ns |    1 | 0.0178 |     224 B |
| MaterializeAndIterateEnumerable2 | 108.8 ns | 1.76 ns | 1.38 ns |    2 | 0.0204 |     256 B |
 */

[RankColumn]
[MemoryDiagnoser]
public class IterateEnumerableVsMaterialized
{
    private static EnumerableTest _testValues = new EnumerableTest
    {
        Values = new Dictionary<string, int>
        {
            {
                "Test1", 1
            },
            {
                "Test2", 2
            },
            {
                "Test3", 3
            },
            {
                "Test4", 4
            }
        }
    };

    [Benchmark]
    public List<int> GetAndIterateEnumerable()
    {
        var numbers = new List<int>();
        var values = _testValues.Values.Select(x => x.Value);
        foreach (var value in values)
        {
            numbers.Add(value);
        }

        return numbers;
    }

    [Benchmark]
    public List<int> MaterializeAndIterateEnumerable()
    {
        var numbers = new List<int>();
        var values = _testValues.Values.Select(x => x.Value).ToArray();
        foreach (var value in values)
        {
            numbers.Add(value);
        }

        return numbers;
    }

    [Benchmark]
    public List<int> MaterializeAndIterateEnumerable2()
    {
        var numbers = new List<int>();
        var values = _testValues.Values.Select(x => x.Value).ToList();
        foreach (var value in values)
        {
            numbers.Add(value);
        }

        return numbers;
    }
}

public class EnumerableTest
{
    public Dictionary<string, int> Values { get; set; }
}