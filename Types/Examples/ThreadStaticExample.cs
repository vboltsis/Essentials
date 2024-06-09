namespace FeatureExamples;

public class ThreadStaticExample
{
    [ThreadStatic]
    private static int _requestCount;

    public static void IncrementRequestCount()
    {
        _requestCount++;
        Console.WriteLine($"Thread ID: {Environment.CurrentManagedThreadId}, Request Count: {_requestCount}");
    }

    public static void Example()
    {
        for (int i = 0; i < 100; i++)
        {
            Task.Run(IncrementRequestCount);
        }

        Thread.Sleep(1000);
    }
}
