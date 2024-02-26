namespace Benchmark;

/*
| Method           | Mean     | Error    | StdDev   | Gen0   | Allocated |
|----------------- |---------:|---------:|---------:|-------:|----------:|
| GetTestTask      | 23.23 ns | 0.487 ns | 0.500 ns | 0.0172 |     144 B |
| GetTestValueTask | 21.99 ns | 0.038 ns | 0.032 ns |      - |         - |
 */

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