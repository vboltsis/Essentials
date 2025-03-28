namespace FeatureExamples.Enumerables;

public class IAsyncEnumerableExample
{
    public async Task ReadFromFileAsync()
    {
        var parentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
            "../../../../notes.txt"));

        // Read the file and display it line by line.
        // await foreach (var line in ReadLinesAsync(parentDirectory))
        // {
        //     Console.WriteLine(line);
        // }

        // await foreach (var line in GetNumbersAsync())
        // {
        //     if (line == 5)
        //     {
        //         break;
        //     }
        //     Console.WriteLine(line);
        // }
        var result = await GetNumbersEnumerableAsync();
        foreach (var number in result)
        {
            if (number == 5)
            {
                break;
            }

            Console.WriteLine(number);
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

    public async IAsyncEnumerable<int> GetNumbersAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(1000);
            yield return i;
        }
    }

    public async Task<IEnumerable<int>> GetNumbersEnumerableAsync()
    {
        var list = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(1000);
            list.Add(i);
        }

        return list;
    }
}
