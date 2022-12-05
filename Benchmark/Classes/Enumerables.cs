using System.Buffers;
using System.Collections;
using System.Collections.ObjectModel;

namespace Benchmark;

/*
|             Method | Number |          Mean |       Error |      StdDev | Ratio | RatioSD |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------------- |------- |--------------:|------------:|------------:|------:|--------:|-------:|-------:|----------:|------------:|
|       ArrayPooling |     10 |     18.734 ns |   0.2179 ns |   0.2039 ns |  1.00 |    0.00 |      - |      - |         - |          NA |
|       GetFixedList |     10 |     22.402 ns |   0.2626 ns |   0.2328 ns |  1.20 |    0.02 | 0.0115 |      - |      96 B |          NA |
|            GetList |     10 |     52.337 ns |   0.5704 ns |   0.5335 ns |  2.79 |    0.05 | 0.0258 |      - |     216 B |          NA |
|           GetArray |     10 |      5.225 ns |   0.0797 ns |   0.0746 ns |  0.28 |    0.01 | 0.0076 |      - |      64 B |          NA |
|      GetStackArray |     10 |      4.410 ns |   0.0175 ns |   0.0163 ns |  0.24 |    0.00 |      - |      - |         - |          NA |
|             GetSet |     10 |    126.071 ns |   0.7671 ns |   0.5989 ns |  6.72 |    0.09 | 0.0792 |      - |     664 B |          NA |
|       GetArrayList |     10 |     94.519 ns |   1.2499 ns |   1.1692 ns |  5.05 |    0.07 | 0.0678 | 0.0001 |     568 B |          NA |
|  GetFixedArrayList |     10 |     66.404 ns |   0.3282 ns |   0.3070 ns |  3.54 |    0.04 | 0.0449 | 0.0001 |     376 B |          NA |
|      GetCollection |     10 |     90.953 ns |   0.3942 ns |   0.3687 ns |  4.86 |    0.05 | 0.0286 |      - |     240 B |          NA |
|      GetDictionary |     10 |    133.913 ns |   0.5697 ns |   0.5329 ns |  7.15 |    0.08 | 0.0927 | 0.0002 |     776 B |          NA |
| GetFixedDictionary |     10 |     72.173 ns |   0.2696 ns |   0.2521 ns |  3.85 |    0.04 | 0.0421 |      - |     352 B |          NA |
|                    |        |               |             |             |       |         |        |        |           |             |
|       ArrayPooling |    100 |     45.362 ns |   0.1292 ns |   0.1145 ns |  1.00 |    0.00 |      - |      - |         - |          NA |
|       GetFixedList |    100 |     92.090 ns |   0.9853 ns |   0.9216 ns |  2.03 |    0.02 | 0.0545 | 0.0001 |     456 B |          NA |
|            GetList |    100 |    185.035 ns |   3.6641 ns |   5.9169 ns |  4.18 |    0.15 | 0.1414 | 0.0005 |    1184 B |          NA |
|           GetArray |    100 |     40.829 ns |   0.8676 ns |   1.8490 ns |  0.91 |    0.04 | 0.0507 | 0.0001 |     424 B |          NA |
|      GetStackArray |    100 |     51.444 ns |   0.2778 ns |   0.2463 ns |  1.13 |    0.01 |      - |      - |         - |          NA |
|             GetSet |    100 |  1,058.488 ns |   5.5857 ns |   5.2248 ns | 23.33 |    0.13 | 0.7172 | 0.0172 |    6000 B |          NA |
|       GetArrayList |    100 |    721.448 ns |   3.6581 ns |   3.2428 ns | 15.90 |    0.08 | 0.5484 | 0.0143 |    4592 B |          NA |
|  GetFixedArrayList |    100 |    619.608 ns |  11.9030 ns |  12.2235 ns | 13.66 |    0.29 | 0.3891 | 0.0086 |    3256 B |          NA |
|      GetCollection |    100 |    664.471 ns |   8.5674 ns |   8.0139 ns | 14.65 |    0.18 | 0.1440 |      - |    1208 B |          NA |
|      GetDictionary |    100 |  1,061.550 ns |  14.6045 ns |  13.6610 ns | 23.41 |    0.30 | 0.8831 | 0.0286 |    7392 B |          NA |
| GetFixedDictionary |    100 |    649.035 ns |   2.3795 ns |   2.1094 ns | 14.31 |    0.06 | 0.2708 | 0.0038 |    2272 B |          NA |
|                    |        |               |             |             |       |         |        |        |           |             |
|       ArrayPooling |   1000 |    301.343 ns |   1.6985 ns |   1.5888 ns |  1.00 |    0.00 |      - |      - |         - |          NA |
|       GetFixedList |   1000 |    839.694 ns |   7.5615 ns |   7.0730 ns |  2.79 |    0.02 | 0.4845 | 0.0134 |    4056 B |          NA |
|            GetList |   1000 |  1,094.846 ns |  10.4651 ns |   8.7388 ns |  3.63 |    0.04 | 1.0052 | 0.0362 |    8424 B |          NA |
|           GetArray |   1000 |    377.947 ns |   2.9776 ns |   2.7852 ns |  1.25 |    0.01 | 0.4807 | 0.0072 |    4024 B |          NA |
|      GetStackArray |   1000 |    467.313 ns |   1.4600 ns |   1.2942 ns |  1.55 |    0.01 |      - |      - |         - |          NA |
|             GetSet |   1000 |  9,892.341 ns | 107.4133 ns | 100.4745 ns | 32.83 |    0.38 | 6.9885 | 1.3885 |   58664 B |          NA |
|       GetArrayList |   1000 |  7,807.446 ns |  57.2436 ns |  53.5457 ns | 25.91 |    0.23 | 4.8523 | 0.9079 |   40600 B |          NA |
|  GetFixedArrayList |   1000 |  5,980.252 ns |  32.5060 ns |  28.8158 ns | 19.84 |    0.13 | 3.8300 | 0.6485 |   32056 B |          NA |
|      GetCollection |   1000 |  5,718.558 ns |  93.1946 ns |  72.7602 ns | 18.96 |    0.27 | 1.0071 | 0.0305 |    8448 B |          NA |
|      GetDictionary |   1000 | 10,067.165 ns |  44.3878 ns |  39.3486 ns | 33.40 |    0.26 | 8.7280 | 2.1667 |   73168 B |          NA |
| GetFixedDictionary |   1000 |  6,311.543 ns |  83.8649 ns |  78.4472 ns | 20.94 |    0.22 | 2.6398 | 0.3738 |   22192 B |          NA |
 */

[MemoryDiagnoser]
public class Enumerables
{
    readonly static ArrayPool<int> _intPool = ArrayPool<int>.Create(1000, 5);

    [Params(10, 100, 1000)]
    public int Number { get; set; }

    [Benchmark(Baseline = true)]
    public void ArrayPooling()
    {
        var rentedArray = _intPool.Rent(Number);
        for (int i = 0; i < Number; i++)
        {
            rentedArray[i] = i;
        }

        _intPool.Return(rentedArray);
    }

    [Benchmark]
    public List<int> GetFixedList()
    {
        var list = new List<int>(Number);
        for (int i = 0; i < Number; i++)
        {
            list.Add(i);
        }
        return list;
    }

    [Benchmark]
    public List<int> GetList()
    {
        var list = new List<int>();
        for (int i = 0; i < Number; i++)
        {
            list.Add(i);
        }
        return list;
    }

    [Benchmark]
    public int[] GetArray()
    {
        var array = new int[Number];
        for (int i = 0; i < Number; i++)
        {
            array[i] = i;
        }
        return array;
    }

    [Benchmark]
    public void GetStackArray()
    {
        Span<int> array = stackalloc int[Number];
        for (int i = 0; i < Number; i++)
        {
            array[i] = i;
        }
    }

    [Benchmark]
    public HashSet<int> GetSet()
    {
        var set = new HashSet<int>();
        for (int i = 0; i < Number; i++)
        {
            set.Add(i);
        }
        return set;
    }

    [Benchmark]
    public ArrayList GetArrayList()
    {
        var list = new ArrayList();
        for (int i = 0; i < Number; i++)
        {
            list.Add(i);
        }
        return list;
    }

    [Benchmark]
    public ArrayList GetFixedArrayList()
    {
        var list = new ArrayList(Number);
        for (int i = 0; i < Number; i++)
        {
            list.Add(i);
        }
        return list;
    }

    [Benchmark]
    public Collection<int> GetCollection()
    {
        var list = new Collection<int>();
        for (int i = 0; i < Number; i++)
        {
            list.Add(i);
        }
        return list;
    }

    [Benchmark]
    public Dictionary<int, int> GetDictionary()
    {
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < Number; i++)
        {
            dict.Add(i, i);
        }
        return dict;
    }

    [Benchmark]
    public Dictionary<int, int> GetFixedDictionary()
    {
        var dict = new Dictionary<int, int>(Number);
        for (int i = 0; i < Number; i++)
        {
            dict.Add(i, i);
        }
        return dict;
    }
}

/*
 
 public static class ToListExtension {

    public static List<T> ToList<T>(this IEnumerable<T> source, int capacity) 
    {
        var res = new List<T>(capacity);
        res.AddRange(source);
        return res;
    }

}
 */