using System.Threading.Channels;

namespace Types;

public class ChannelTaskQueue
{
    private readonly Channel<Func<Task>> taskChannel = Channel.CreateUnbounded<Func<Task>>();

    public async Task EnqueueTask(Func<Task> taskFunc)
    {
        await taskChannel.Writer.WriteAsync(taskFunc);
    }

    public async Task TaskConsumer()
    {
        await foreach (var taskFunc in taskChannel.Reader.ReadAllAsync())
        {
            await taskFunc();
        }
    }

    public async Task StartTaskConsumers(int numberOfConsumers)
    {
        var consumers = new List<Task>(numberOfConsumers);

        for (int i = 0; i < numberOfConsumers; i++)
        {
            consumers.Add(TaskConsumer());
        }

        await Task.WhenAll(consumers);
    }
}
