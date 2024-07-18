namespace FeatureExamples;

internal class LockingExamples
{
    private static object _lock = new object();

    public static void Example()
    {
        for (int i = 0; i < 10; i++)
        {
            Task.Run(DoWorkMonitor);
        }
    }

    public static void DoWork()
    {
        lock (_lock)
        {
            Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} started");
            Thread.Sleep(1000);
            var randomNumber = Random.Shared.Next(1, 5);

            if (randomNumber == 3)
            {
                throw new Exception("Random number is 3");
            }

            Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} finished");
        }
    }

    public static void DoWorkMonitor()
    {
        Monitor.Enter(_lock);
            Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} started");
            Thread.Sleep(1000);
            var randomNumber = Random.Shared.Next(1, 5);

            if (randomNumber == 3)
            {
                throw new Exception("Random number is 3");
            }

            Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} finished");
        Monitor.Exit(_lock);
    }
}
