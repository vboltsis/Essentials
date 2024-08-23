namespace XUnit.Tests;

[CollectionDefinition(nameof(ParallelTest), DisableParallelization = false)]
public class ParallelTest
{
    [Fact]
    public void Test1()
    {
        Thread.Sleep(1000);
    }
    [Fact]
    public void Test2()
    {
        Thread.Sleep(1000);
    }
    [Fact]
    public void Test3()
    {
        Thread.Sleep(1000);
    }
}
