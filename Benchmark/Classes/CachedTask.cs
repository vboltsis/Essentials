[MemoryDiagnoser] 
public class CachedTaskBenchmark
{
    private static readonly object _cachedObject = new();
    private static readonly Dictionary<string, object> _cachedObjects = new()
        { { "policy", _cachedObject } };
    private static readonly Dictionary<string, Task<object>> _cachedTasks = new()
        { { "policy", Task.FromResult(_cachedObject) } };
    private static readonly Dictionary<string, ValueTask<object>> _cachedValueTasks = new()
        { { "policy", ValueTask.FromResult(_cachedObject) } };

    [Benchmark(Baseline = true)]
    public Task<object> GetTask()
    {
        return Task.FromResult(_cachedObjects["policy"]);
    }

    [Benchmark]
    public ValueTask<object> GetValueTask()
    {
        return ValueTask.FromResult(_cachedObjects["policy"]);
    }

    [Benchmark]
    public Task<object> GetCachedTask()
    {
        return _cachedTasks["policy"];
    }

    [Benchmark]
    public ValueTask<object> GetCachedValueTask()
    {
        return _cachedValueTasks["policy"];
    }
}