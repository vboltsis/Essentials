using System.Collections.Frozen;

namespace Benchmark;

/*
| Method            | Size | Mean     | Error     | StdDev    |
|------------------ |----- |---------:|----------:|----------:|
| HashSetContains   | 1000 | 3.102 ns | 0.0103 ns | 0.0091 ns |
| FrozenSetContains | 1000 | 2.292 ns | 0.0199 ns | 0.0177 ns |
*/

[MemoryDiagnoser]
public class FrozenSetVsHashSet
{
    private Random _random = null!;
    private HashSet<int> _hashSet = null!;
    private FrozenSet<int> _frozenSet = null!;

    private int _number;

    [Params(1000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _random = new Random(69);

        _hashSet = Enumerable.Range(0, Size).Select(_ => _random.Next()).ToHashSet();
        _frozenSet = _hashSet.ToFrozenSet();
        _number = _random.Next();
    }

    [Benchmark]
    public bool HashSetContains()
    {
        return _hashSet.Contains(_number);
    }

    [Benchmark]
    public bool FrozenSetContains()
    {
        return _frozenSet.Contains(_number);
    }
}
