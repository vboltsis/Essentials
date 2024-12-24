namespace Benchmark.Classes;

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class LockVsMonitor
{
    private static object _lock = new object();

    [Benchmark]
    public int LockMethod()
    {
        for (int i = 0; i < 10; i++)
        {
            Task.Run(DoWork);
        }

        Thread.Sleep(100);

        return 1;
    }

    [Benchmark]
    public int MonitorMethod()
    {
        for (int i = 0; i < 10; i++)
        {
            Task.Run(DoWorkMonitor);
        }

        Thread.Sleep(100);

        return 1;
    }

    public void DoWork()
    {
        lock (_lock)
        {
            Thread.Sleep(10);
        }
    }

    public void DoWorkMonitor()
    {
        Monitor.Enter(_lock);
        Thread.Sleep(10);
        Monitor.Exit(_lock);
    }
}