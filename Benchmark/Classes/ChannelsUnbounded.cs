using System.Threading.Channels;

namespace Benchmark;

/*
docker build -t benchmark .
*/

/*
--WINDOWS BENCHMARK--
|                      Method |     Mean |    Error |   StdDev |   Gen0 | Allocated |
|---------------------------- |---------:|---------:|---------:|-------:|----------:|
| ParallelWriteAndReadChannel | 47.87 us | 0.941 us | 1.465 us | 0.0610 |     600 B |
*/

[MemoryDiagnoser]
public class ChannelsUnbounded
{
    private Channel<int> _channel = Channel.CreateUnbounded<int>();

    [Benchmark]
    public async Task ParallelWriteAndReadChannel()
    {
        var writer = Task.Run(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                _channel.Writer.TryWrite(i);
            }
        });

        var reader = Task.Run(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                _channel.Reader.TryRead(out _);
            }
        });

        await Task.WhenAll(writer, reader);
    }
}
