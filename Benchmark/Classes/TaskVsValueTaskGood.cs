namespace Benchmark;

/*
.NET 8 result
| Method           | Mean     | Error    | StdDev   | Gen0   | Allocated |
|----------------- |---------:|---------:|---------:|-------:|----------:|
| GetTestTask      | 24.71 ns | 0.510 ns | 0.452 ns | 0.0172 |     144 B |
| GetTestValueTask | 19.87 ns | 0.072 ns | 0.064 ns |      - |         - |
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