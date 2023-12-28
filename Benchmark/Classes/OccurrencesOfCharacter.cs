namespace Benchmark;

/*
| Method                        | Input                | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------------------------ |--------------------- |---------:|---------:|---------:|-------:|----------:|
| GetOccurencesWithLinq         | Testa(...)hvhgv [25] | 33.89 ns | 0.281 ns | 0.249 ns | 0.0025 |      32 B |
| GetOccurencesWithLoop         | Testa(...)hvhgv [25] | 14.86 ns | 0.317 ns | 0.474 ns |      - |         - |
| GetOccurencesWithLoopAndSpan  | Testa(...)hvhgv [25] | 15.10 ns | 0.074 ns | 0.065 ns |      - |         - |
| GetOccurencesWithLoopAndSpan2 | Testa(...)hvhgv [25] | 11.71 ns | 0.225 ns | 0.211 ns |      - |         - |
 */

[MemoryDiagnoser]
public class OccurrencesOfCharacter
{
    [Params("Testashfasfwweasdfaghvhgv")]
    public string Input { get; set; }

    [Benchmark]
    public int GetOccurencesWithLinq()
    {
        return Input.Count(x => x == 'a');
    }

    [Benchmark]
    public int GetOccurencesWithLoop()
    {
        var count = 0;
        foreach (var character in Input)
        {
            if (character == 'a')
            {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int GetOccurencesWithLoopAndSpan()
    {
        var count = 0;
        var span = Input.AsSpan();
        for (var i = 0; i < span.Length; i++)
        {
            if (span[i] == 'a')
            {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int GetOccurencesWithLoopAndSpan2()
    {
        var count = 0;
        var span = Input.AsSpan();
        for (var i = span.Length - 1; i >= 0; i--)
        {
            if (span[i] == 'a')
            {
                count++;
            }
        }

        return count;
    }
}
