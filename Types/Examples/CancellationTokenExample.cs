namespace FeatureExamples;

public class CancellationTokenExample
{
    public static void Example()
    {
        try
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine("Running...");
                    Thread.Sleep(1000);
                }
            });

            Console.WriteLine("Press any key to cancel the operation");
            Console.ReadKey();
            cts.Cancel();
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"The operation was cancelled. {ex}");
        }
    }

    public static async Task TimeoutExample()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        cts.CancelAfter(TimeSpan.FromSeconds(5));

        var task = Task.Run(() => DoWork(token), token);

        try
        {
            await task;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task was cancelled due to timeout.");
        }
    }

    public static async Task LinkTokensExample()
    {
        using var cts1 = new CancellationTokenSource();
        using var cts2 = new CancellationTokenSource();
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);

        var token = linkedCts.Token;

        var task = Task.Run(() => DoWork(token), token);

        // Simulate user canceling from either source
        await Task.Delay(2000);
        cts1.Cancel();

        try
        {
            await task;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task was cancelled from one of the sources.");
        }
    }

    static void DoWork(CancellationToken token)
    {
        for (int i = 0; i < 10; i++)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine($"Working... {i}");
            Thread.Sleep(1000); // Simulate work
        }
    }
}
