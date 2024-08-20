namespace XUnit.Tests;

public class ParallelTest
{
    [Fact]
    public void Test1()
    {
        Thread.Sleep(5000);
    }
    [Fact]
    public void Test2()
    {
        Thread.Sleep(5000);
    }
    [Fact]
    public void Test3()
    {
        Thread.Sleep(5000);
    }
}
