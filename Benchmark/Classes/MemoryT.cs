using System.Text;

namespace Benchmark.Classes;

public class MemoryT
{
    public async Task Process()
    {
        // Open the file
        using (var file = File.OpenRead("largefile.txt"))
        {
            // Allocate a buffer to hold the data
            var buffer = new byte[8192];
            var memory = new Memory<byte>(buffer);

            // Read the file in chunks
            int bytesRead;
            while ((bytesRead = await file.ReadAsync(memory)) > 0)
            {
                // Process the current chunk of data
                ProcessChunk(memory.Slice(0, bytesRead));
            }
        }
    }

    private static void ProcessChunk(Memory<byte> chunk)
    {
        // do something with the data
        Console.WriteLine(Encoding.UTF8.GetString(chunk.ToArray()));
    }
}
