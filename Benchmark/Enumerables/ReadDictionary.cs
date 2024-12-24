namespace Benchmark.Classes;

/*
| Method           | Mean      | Error     | StdDev    | Allocated |
|----------------- |----------:|----------:|----------:|----------:|
| ReadWithTryGet   |  8.324 ns | 0.1932 ns | 0.1807 ns |         - |
| ReadWithContains | 12.791 ns | 0.2534 ns | 0.2370 ns |         - | 
*/

[MemoryDiagnoser] 
public class ReadDictionary
{
    private static Dictionary<int, string> _dictionary = new();

    public ReadDictionary()
    {
        for (var i = 0; i <= 10_000; i++)
        {
            _dictionary.Add(i, i.ToString());
        }
    }

    [Benchmark]
    public string ReadWithTryGet()
    {
        var randomKey = Random.Shared.Next(1, 10_001);

        if (_dictionary.TryGetValue(randomKey, out var value))
        {
            return value;
        }

        return string.Empty;
    }

    [Benchmark]
    public string ReadWithContains()
    {
        var randomKey = Random.Shared.Next(1, 10_001);
        if (_dictionary.ContainsKey(randomKey))
        {
            return _dictionary[randomKey];
        }

        return string.Empty;
    }
}
