namespace Benchmark.Classes;
/*
| Method        | Number | Mean       | Error     | StdDev    | Allocated |
|-------------- |------- |-----------:|----------:|----------:|----------:|
| ContainsSet   | 3      |   3.213 ns | 0.0893 ns | 0.0993 ns |         - |
| ContainsList  | 3      |   2.543 ns | 0.0769 ns | 0.0682 ns |         - |
| ContainsArray | 3      |   3.142 ns | 0.0884 ns | 0.2083 ns |         - |

| ContainsSet   | 10     |   3.272 ns | 0.0933 ns | 0.1111 ns |         - |
| ContainsList  | 10     |   3.066 ns | 0.0828 ns | 0.0953 ns |         - |
| ContainsArray | 10     |   3.514 ns | 0.0950 ns | 0.1478 ns |         - |

| ContainsSet   | 100    |   3.242 ns | 0.0609 ns | 0.0540 ns |         - |
| ContainsList  | 100    |   5.518 ns | 0.0990 ns | 0.0926 ns |         - |
| ContainsArray | 100    |   5.840 ns | 0.0685 ns | 0.0572 ns |         - |

| ContainsSet   | 1000   |   3.529 ns | 0.0323 ns | 0.0286 ns |         - |
| ContainsList  | 1000   |  25.878 ns | 0.5458 ns | 0.7651 ns |         - |
| ContainsArray | 1000   |  26.283 ns | 0.5379 ns | 0.5031 ns |         - |

| ContainsSet   | 10000  |   3.399 ns | 0.0953 ns | 0.1097 ns |         - |
| ContainsList  | 10000  | 224.015 ns | 4.1378 ns | 3.4552 ns |         - |
| ContainsArray | 10000  | 197.988 ns | 3.9709 ns | 4.5729 ns |         - |
 */

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class ContainsSetVsListVsArray
{
    private HashSet<int> _set;
    private List<int> _list;
    private int[] _array;

    [Params(3, 10, 100, 1000, 10000)]
    public int Number;

    public int ContainedNumber => Number / 2;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _set = new HashSet<int>(Enumerable.Range(1, Number));
        _list = new List<int>(Enumerable.Range(1, Number));
        _array = Enumerable.Range(1, Number).ToArray();
    }

    [Benchmark]
    public bool ContainsSet()
    {
        return _set.Contains(ContainedNumber);
    }
    
    [Benchmark]
    public bool ContainsList()
    {
        return _list.Contains(ContainedNumber);
    }    
    
    [Benchmark]
    public bool ContainsArray()
    {
        return _array.Contains(ContainedNumber);
    }
}
