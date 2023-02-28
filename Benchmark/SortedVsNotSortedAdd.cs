namespace Benchmark;
/*
|              Method |  Number |       Mean |     Error |    StdDev | Rank |      Gen0 |      Gen1 |     Gen2 | Allocated |
|-------------------- |-------- |-----------:|----------:|----------:|-----:|----------:|----------:|---------:|----------:|
|        SortedSetAdd |  100000 |   9.333 ms | 0.0487 ms | 0.0407 ms |    3 |  312.5000 |  296.8750 |        - |   3.81 MB |
|              SetAdd |  100000 |   1.419 ms | 0.0190 ms | 0.0149 ms |    1 |  595.7031 |  580.0781 | 578.1250 |   4.61 MB |
| SortedDictionaryAdd |  100000 |  16.006 ms | 0.1032 ms | 0.0965 ms |    6 |  375.0000 |  343.7500 |        - |   4.58 MB |
|       DictionaryAdd |  100000 |   1.580 ms | 0.0222 ms | 0.0208 ms |    2 |  591.7969 |  576.1719 | 572.2656 |   5.76 MB |
|        SortedSetAdd | 1000000 | 146.378 ms | 1.6365 ms | 1.3666 ms |    7 | 3500.0000 | 3250.0000 | 500.0000 |  38.15 MB |
|              SetAdd | 1000000 |  11.031 ms | 0.1409 ms | 0.1249 ms |    4 |  796.8750 |  781.2500 | 781.2500 |  41.11 MB |
| SortedDictionaryAdd | 1000000 | 224.967 ms | 2.4999 ms | 2.2161 ms |    8 | 4333.3333 | 4000.0000 | 666.6667 |  45.78 MB |
|       DictionaryAdd | 1000000 |  12.405 ms | 0.2476 ms | 0.2752 ms |    5 |  640.6250 |  625.0000 | 625.0000 |  51.39 MB |
 */


[RankColumn]
[MemoryDiagnoser]
public class SortedVsNotSortedAdd {

    [Params(100_000, 1_000_000)]
    public int Number;

    [Benchmark]
    public SortedSet<int> SortedSetAdd() {
        var set = new SortedSet<int>();
        for (int i = 0; i < Number; i++) {
            set.Add(i);
        }

        return set;
    }

    [Benchmark]
    public HashSet<int> SetAdd() {
        var set = new HashSet<int>();
        for (int i = 0; i < Number; i++) {
            set.Add(i);
        }

        return set;
    }

    [Benchmark]
    public SortedDictionary<int, int> SortedDictionaryAdd() {
        var set = new SortedDictionary<int, int>();
        for (int i = 0; i < Number; i++) {
            set.Add(i, i);
        }

        return set;
    }

    [Benchmark]
    public Dictionary<int, int> DictionaryAdd() {
        var set = new Dictionary<int, int>();
        for (int i = 0; i < Number; i++) {
            set.Add(i, i);
        }

        return set;
    }
}
