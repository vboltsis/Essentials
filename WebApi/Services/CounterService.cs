namespace WebApi;

public class CounterService : ICounterService,
    ITransientCounterService, IScopedCounterService, ISingletonCounterService
{
    private int _count;

    public int IncreaseAndGet()
    {
        _count++;
        return _count;
    }

    public CounterService()
    {
        Console.WriteLine("CounterService created");
    }
}

public interface IAnotherService
{
    int GetScopedCount();
}

public class AnotherService : IAnotherService
{
    private readonly IScopedCounterService _transientCounter;

    public AnotherService(IScopedCounterService transientCounter)
    {
        _transientCounter = transientCounter;
    }

    public int GetScopedCount()
    {
        return _transientCounter.IncreaseAndGet();
    }
}

public interface ICounterService
{
    int IncreaseAndGet();
}

public interface ITransientCounterService
{
    int IncreaseAndGet();
}
public interface IScopedCounterService
{
    int IncreaseAndGet();
}
public interface ISingletonCounterService
{
    int IncreaseAndGet();
}