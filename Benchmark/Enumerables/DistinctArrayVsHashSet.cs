namespace Benchmark;

/*
|        Method | Number |     Mean |    Error |   StdDev |   Gen0 |   Gen1 | Allocated |
|-------------- |------- |---------:|---------:|---------:|-------:|-------:|----------:|
| DistinctArray |      2 | 226.1 ns |  4.15 ns |  3.68 ns | 0.0522 |      - |     656 B |
|  DistinctList |      2 | 244.9 ns |  4.60 ns |  8.41 ns | 0.0548 |      - |     688 B |
|       HashSet |      2 | 219.6 ns |  4.40 ns |  8.89 ns | 0.0420 |      - |     528 B |
| DistinctArray |      5 | 519.4 ns |  9.21 ns |  8.16 ns | 0.1163 |      - |    1464 B |
|  DistinctList |      5 | 537.1 ns | 10.21 ns | 12.16 ns | 0.1192 |      - |    1496 B |
|       HashSet |      5 | 504.0 ns | 10.09 ns | 20.85 ns | 0.1059 |      - |    1336 B |
| DistinctArray |     10 | 905.5 ns |  5.10 ns |  3.98 ns | 0.1774 | 0.0010 |    2232 B |
|  DistinctList |     10 | 972.4 ns | 18.93 ns | 25.27 ns | 0.1802 | 0.0010 |    2264 B |
|       HashSet |     10 | 910.5 ns | 17.02 ns | 18.21 ns | 0.1669 | 0.0010 |    2104 B |
 */

[MemoryDiagnoser]
public class DistinctArrayVsHashSet
{
    private List<int> Numbers;

    //number of duplicates * 10. So 2 means 20 duplicates.
    [Params(2, 5, 10)]
    public int Number;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Numbers = FillList();
    }

    [Benchmark]
    public int[] DistinctArray()
    {
        return Numbers.Distinct().ToArray();
    }

    [Benchmark]
    public List<int> DistinctList()
    {
        return Numbers.Distinct().ToList();
    }

    [Benchmark]
    public HashSet<int> HashSet()
    {
        return Numbers.ToHashSet();
    }

    private List<int> FillList()
    {
        var list = new List<int>(Number);
        for (int i = 0; i < Number; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                list.Add(j);
            }
        }
        return list;
    }
}
