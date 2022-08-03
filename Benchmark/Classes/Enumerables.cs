using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections;
using System.Collections.ObjectModel;

namespace Benchmark;

internal class Enumerables
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
