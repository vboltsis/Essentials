namespace FeatureExamples;

public class SemaphoreExample
{
    // Create a SemaphoreSlim that allows up to 3 concurrent threads.
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(3);

    public static async Task Example()
    {
        Task[] tasks = new Task[10];
        for (int i = 0; i < tasks.Length; i++)
        {
            int taskId = i; // Local copy for closure safety
            tasks[i] = Task.Run(() => DoWorkAsync(taskId));
        }

        await Task.WhenAll(tasks);
        Console.WriteLine("All tasks have completed.");
    }

    static async Task DoWorkAsync(int id)
    {
        Console.WriteLine($"Task {id} is waiting to enter...");
        await semaphore.WaitAsync();
        try
        {
            Console.WriteLine($"Task {id} entered the semaphore.");
            // Simulate some work.
            await Task.Delay(10000);
        }
        finally
        {
            semaphore.Release();
            Console.WriteLine($"Task {id} released the semaphore.");
        }
    }
}
