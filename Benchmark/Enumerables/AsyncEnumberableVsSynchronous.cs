namespace Benchmark;

/*
| Method            | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------ |---------:|---------:|---------:|-------:|-------:|----------:|
| ReadFromFileAsync | 61.90 us | 0.607 us | 0.538 us | 2.1973 |      - |  16.09 KB |
| ReadFromFile      | 26.09 us | 0.169 us | 0.150 us | 1.4954 | 0.0610 |  12.24 KB |
 */

[MemoryDiagnoser]
public class AsyncEnumberableVsSynchronous
{
    private static readonly string parentDirectory = Path.GetFullPath("C:\\Users\\Evangelos\\Desktop\\Code\\Essentials\\notes.txt");

    [Benchmark]
    public async Task<List<string>> ReadFromFileAsync()
    {
        var lines = new List<string>();

        await foreach (var line in ReadLinesAsync(parentDirectory))
        {
            lines.Add(line);
        }

        return lines;
    }

    [Benchmark]
    public List<string> ReadFromFile()
    {
        var lines = new List<string>();

        foreach (var line in File.ReadLines(parentDirectory))
        {
            lines.Add(line);
        }

        return lines;
    }

    async IAsyncEnumerable<string> ReadLinesAsync(string filePath)
    {
        using var reader = File.OpenText(filePath);
        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            yield return line;
        }
    }
}
