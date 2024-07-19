namespace FeatureExamples;

internal class ManualResetEventExample
{
    static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

    public static void Write()
    {
        Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} writing");
        _manualResetEvent.Reset();
        Thread.Sleep(10000);
        Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} finished writing");
        _manualResetEvent.Set();
    }

    public static void Read()
    {
        Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} waiting");
        _manualResetEvent.WaitOne();
        Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId} reading");
    }

    public static void Example()
    {
        Task.Run(Write);
        
        for (int i = 0; i < 5; i++)
        {
            Task.Run(Read);
        }

        _manualResetEvent.WaitOne();
    }
}
