using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Types;

public class ChannelExamples
{
    public static async Task ChannelUnboundedExample()
    {
        var channel = Channel.CreateUnbounded<int>();

        _ = Task.Run(async () =>
        {
            for (var i = 0; ; i++)
            {
                await Task.Delay(1000);
                channel.Writer.TryWrite(i);
            }
        });

        while (true)
        {
            var item = await channel.Reader.ReadAsync();
            Console.WriteLine(item);
        }
    }

    public static async Task ChannelBoundedExample()
    {
        var options = new BoundedChannelOptions(10)
        {
            AllowSynchronousContinuations = false,
            Capacity = 10,
            SingleReader = false,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
            //FullMode = BoundedChannelFullMode.DropWrite
            //FullMode = BoundedChannelFullMode.DropNewest
            //FullMode = BoundedChannelFullMode.DropOldest
        };

        var channel = Channel.CreateBounded<int>(options);

        _ = Task.Run(async () =>
        {
            for (var i = 0; ; i++)
            {
                await Task.Delay(1000);
                channel.Writer.TryWrite(i);
            }
        });

        while (true)
        {
            var item = await channel.Reader.ReadAsync();
            Console.WriteLine(item);
        }
    }

    public static async Task MyChannel()
    {
        var channel = new MyChannel<int>();

        _ = Task.Run(async () =>
        {
            for (var i = 0; ; i++)
            {
                await Task.Delay(1000);
                channel.Write(i);
            }
        });

        while (true)
        {
            var item = await channel.ReadAsync();
            Console.WriteLine(item);
        }
    }
}

public class MyChannel<T>
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly SemaphoreSlim _semaphore = new(0);

    public void Write(T item)
    {
        _queue.Enqueue(item);
        _semaphore.Release();
    }

    public async Task<T> ReadAsync()
    {
        await _semaphore.WaitAsync();
        _queue.TryDequeue(out var item);
        return item;
    }
}

/*
https://www.youtube.com/watch?v=gT06qvQLtJ0
https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/?WT.mc_id=ondotnet-c9-cephilli 
*/