using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace Benchmark.Classes;

[MemoryDiagnoser]
public class ReadDictionary
{
    private readonly ReaderWriterLockSlim _slimLock = new();
    private static Dictionary<int, string> _dictionary = new();

    public ReadDictionary()
    {
        for (var i = 0; i <= 10_000; i++)
        {
            _dictionary.Add(i, i.ToString());
        }
    }

    //[Benchmark]
    //public string ReadWithLock()
    //{
    //    var randomKey = new Random().Next(1, 10_001);
    //    _slimLock.EnterReadLock();
    //    try
    //    {
    //        return _dictionary[randomKey];
    //    }
    //    finally
    //    {
    //        _slimLock.ExitReadLock();
    //    }
    //}

    [Benchmark]
    public string ReadWithTryGet()
    {
        var randomKey = new Random().Next(1, 10_001);

        if (_dictionary.TryGetValue(randomKey, out var value))
        {
            return value;
        }

        return string.Empty;
    }

    [Benchmark]
    public string ReadWithContains()
    {
        var randomKey = new Random().Next(1, 10_001);
        if (_dictionary.ContainsKey(randomKey))
        {
            return _dictionary[randomKey];
        }

        return string.Empty;
    }

    //[Benchmark]
    //public string ReadWithSequence()
    //{
    //    var sequenceReader = new SequenceReader<string>(_dictionary);
    //    var randomKey = new Random().Next(1, 10_001);
    //    return _dictionary[randomKey];
    //}

}
