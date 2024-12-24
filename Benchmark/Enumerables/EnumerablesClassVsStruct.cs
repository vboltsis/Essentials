using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections;
using System.Collections.ObjectModel;

namespace Benchmark;

/*
|             Method |        Mean |     Error |    StdDev |  Ratio | RatioSD |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------------- |------------:|----------:|----------:|-------:|--------:|-------:|-------:|----------:|------------:|
|       ArrayPooling |    28.39 ns |  0.166 ns |  0.155 ns |   1.00 |    0.00 |      - |      - |         - |          NA |
|       GetFixedList |    32.84 ns |  0.520 ns |  0.487 ns |   1.16 |    0.02 | 0.0449 | 0.0001 |     376 B |          NA |
|            GetList |    78.58 ns |  1.618 ns |  1.589 ns |   2.77 |    0.06 | 0.1194 | 0.0005 |    1000 B |          NA |
|           GetArray |    24.50 ns |  0.548 ns |  0.652 ns |   0.87 |    0.02 | 0.0411 | 0.0000 |     344 B |          NA |
|      GetStackArray |    13.78 ns |  0.085 ns |  0.079 ns |   0.49 |    0.00 |      - |      - |         - |          NA |
|             GetSet | 7,106.30 ns | 60.167 ns | 56.280 ns | 250.34 |    2.87 | 1.2970 | 0.0076 |   10888 B |          NA |
|       GetArrayList |   104.79 ns |  0.415 ns |  0.368 ns |   3.69 |    0.02 | 0.0966 | 0.0004 |     808 B |          NA |
|  GetFixedArrayList |    80.53 ns |  0.274 ns |  0.229 ns |   2.84 |    0.02 | 0.0736 | 0.0002 |     616 B |          NA |
|      GetCollection |   127.30 ns |  1.159 ns |  1.084 ns |   4.48 |    0.04 | 0.1223 | 0.0005 |    1024 B |          NA |
|      GetDictionary |   170.05 ns |  0.454 ns |  0.425 ns |   5.99 |    0.04 | 0.1960 | 0.0014 |    1640 B |          NA |
| GetFixedDictionary |    92.15 ns |  1.898 ns |  2.783 ns |   3.30 |    0.12 | 0.0842 | 0.0004 |     704 B |          NA |
 */

[MemoryDiagnoser] 
public class EnumerablesClassVsStruct
{
    readonly static ArrayPool<Coordinates> _intPool = ArrayPool<Coordinates>.Create(1000, 5);
    const int number = 10;

    private Coordinates[] Numbers { get; set; } =
    [
        new Coordinates
        {
            X = 1.3M,
            Y = 1.3M
        },
        new Coordinates
        {
            X = 2.3M,
            Y = 2.3M
        },
        new Coordinates
        {
            X = 3.3M,
            Y = 3.3M
        },
        new Coordinates
        {
            X = 4.3M,
            Y = 4.3M
        },
        new Coordinates
        {
            X = 5.3M,
            Y = 5.3M
        },
        new Coordinates
        {
            X = 6.3M,
            Y = 6.3M
        },
        new Coordinates
        {
            X = 7.3M,
            Y = 7.3M
        },
        new Coordinates
        {
            X = 8.3M,
            Y = 8.3M
        },
        new Coordinates
        {
            X = 9.3M,
            Y = 9.3M
        },
        new Coordinates
        {
            X = 10.3M,
            Y = 10.3M
        }
    ];

    [Benchmark(Baseline = true)]
    public void ArrayPooling()
    {
        var rentedArray = _intPool.Rent(number);
        for (int i = 0; i < Numbers.Length; i++)
        {
            rentedArray[i] = Numbers[i];
        }

        _intPool.Return(rentedArray);
    }

    [Benchmark]
    public List<Coordinates> GetFixedList()
    {
        var list = new List<Coordinates>(number);
        for (int i = 0; i < Numbers.Length; i++)
        {
            list.Add(Numbers[i]);
        }
        return list;
    }

    [Benchmark]
    public List<Coordinates> GetList()
    {
        var list = new List<Coordinates>();
        for (int i = 0; i < Numbers.Length; i++)
        {
            list.Add(Numbers[i]);
        }
        return list;
    }

    [Benchmark]
    public Coordinates[] GetArray()
    {
        var array = new Coordinates[number];
        for (int i = 0; i < Numbers.Length; i++)
        {
            array[i] = Numbers[i];
        }
        return array;
    }

    [Benchmark]
    public void GetStackArray()
    {
        Span<Coordinates> array = stackalloc Coordinates[number];
        for (int i = 0; i < Numbers.Length; i++)
        {
            array[i] = Numbers[i];
        }
    }

    [Benchmark]
    public HashSet<Coordinates> GetSet()
    {
        var set = new HashSet<Coordinates>();
        for (int i = 0; i < Numbers.Length; i++)
        {
            set.Add(Numbers[i]);
        }
        return set;
    }

    [Benchmark]
    public ArrayList GetArrayList()
    {
        var list = new ArrayList();
        for (int i = 0; i < Numbers.Length; i++)
        {
            list.Add(Numbers[i]);
        }
        return list;
    }

    [Benchmark]
    public ArrayList GetFixedArrayList()
    {
        var list = new ArrayList(number);
        for (int i = 0; i < Numbers.Length; i++)
        {
            list.Add(Numbers[i]);
        }
        return list;
    }

    [Benchmark]
    public Collection<Coordinates> GetCollection()
    {
        var list = new Collection<Coordinates>();
        for (int i = 0; i < Numbers.Length; i++)
        {
            list.Add(Numbers[i]);
        }
        return list;
    }

    [Benchmark]
    public Dictionary<int, Coordinates> GetDictionary()
    {
        var dict = new Dictionary<int, Coordinates>();
        for (int i = 0; i < Numbers.Length; i++)
        {
            dict.Add(i, Numbers[i]);
        }
        return dict;
    }

    [Benchmark]
    public Dictionary<int, Coordinates> GetFixedDictionary()
    {
        var dict = new Dictionary<int, Coordinates>(number);
        for (int i = 0; i < Numbers.Length; i++)
        {
            dict.Add(i, Numbers[i]);
        }
        return dict;
    }
}
public struct Coordinates
{
    public decimal X { get; set; }
    public decimal Y { get; set; }
}
