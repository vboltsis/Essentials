namespace FeatureExamples;

public class IAsyncEnumerableExample
{
    public async Task ReadFromFileAsync()
    {
        var parentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
            "../../../../notes.txt"));

        // Read the file and display it line by line.
        await foreach (var line in ReadLinesAsync(parentDirectory))
        {
            Console.WriteLine(line);
        }

        async IAsyncEnumerable<string> ReadLinesAsync(string filePath)
        {
            using var reader = File.OpenText(filePath);
            string? line;
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
            await Task.Delay(100);
            yield return i;
        }
    }
}
