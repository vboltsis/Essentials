namespace Benchmark.Classes;

[MemoryDiagnoser]
public class ValueTaskNested
{
    private static readonly Test _test = new Test { Name = "BenchmarkTest" };

    [Benchmark]
    public async Task<Test> NestedWithTask()
    {
        return await TaskLevel1Async();
    }

    [Benchmark]
    public async ValueTask<Test> NestedWithValueTask()
    {
        return await ValueTaskLevel1Async();
    }

    private async Task<Test> TaskLevel1Async()
    {
        return await TaskLevel2Async();
    }

    private async Task<Test> TaskLevel2Async()
    {
        return await TaskLevel3Async();
    }

    private async Task<Test> TaskLevel3Async()
    {
        await Task.Delay(1);
        return _test;
    }

    private async ValueTask<Test> ValueTaskLevel1Async()
    {
        return await ValueTaskLevel2Async();
    }

    private async ValueTask<Test> ValueTaskLevel2Async()
    {
        return await ValueTaskLevel3Async();
    }

    private async ValueTask<Test> ValueTaskLevel3Async()
    {
        await Task.Delay(1);
        return _test;
    }
}

public class Test
{
    public string Name { get; set; }
}