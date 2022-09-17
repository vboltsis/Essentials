using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections;
using System.Collections.ObjectModel;

namespace Benchmark;

internal struct Coordinates
{
    public decimal X { get; set; }
    public decimal Y { get; set; }
}

internal class EnumerablesClassVsStruct
{
    readonly static ArrayPool<Coordinates> _intPool = ArrayPool<Coordinates>.Create(1000, 5);
    const int number = 10;

    private Coordinates[] Numbers { get; set; } = new Coordinates[]
    {
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
    };

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
