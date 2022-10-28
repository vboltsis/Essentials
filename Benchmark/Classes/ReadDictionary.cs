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

    [Benchmark]
    public string ReadWithLock()
    {
        var randomKey = new Random().Next(1, 10_001);
        _slimLock.EnterReadLock();
        try
        {
            return _dictionary[randomKey];
        }
        finally
        {
            _slimLock.ExitReadLock();
        }
    }

    [Benchmark]
    public string Read()
    {
        var randomKey = new Random().Next(1, 10_001);
        return _dictionary[randomKey];
    }

    //[Benchmark]
    //public string ReadWithSequence()
    //{
    //    var sequenceReader = new SequenceReader<string>(_dictionary);
    //    var randomKey = new Random().Next(1, 10_001);
    //    return _dictionary[randomKey];
    //}

}
