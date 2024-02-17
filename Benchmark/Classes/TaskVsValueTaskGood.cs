namespace Benchmark;

[MemoryDiagnoser]
public class TaskVsValueTaskGood
{
    private static Test _test = new Test();

    [Benchmark]
    public async Task<Test> GetTestTask()
    {
        return await GetTest();
    }

    [Benchmark]
    public async ValueTask<Test> GetTestValueTask()
    {
        return await GetTestValue();
    }

    private async Task<Test> GetTest()
    {
        await Task.CompletedTask;

        return _test;
    }

    private async ValueTask<Test> GetTestValue()
    {
        await ValueTask.CompletedTask;

        return _test;
    }
}

public class Test
{
    public string Name { get; set; }
}