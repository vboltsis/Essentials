namespace Benchmark.Classes;

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public class InitialCapacityClass
{
    private List<CapacityClass> _testClass;

    [Params(8, 32, 128, 256, 512)]
    public int _number;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _testClass = CapacityClass.GetList(512);
    }

    [Benchmark(Baseline = true)]
    public void AddToList()
    {
        var list = new List<CapacityClass>();

        foreach (var item in _testClass)
        {
            list.AddRange(new[] { item });
        }
    }

    [Benchmark]
    public void AddToCapacityList()
    {
        var list = new List<CapacityClass>(_number);

        foreach (var item in _testClass)
        {
            list.AddRange(new[] { item });
        }
    }

}

public class CapacityClass
{
    public long Id { get; set; }
    public bool Canceled { get; set; }
    public long FeedIncidentId { get; set; }
    public ClassType ClassTypeId { get; set; }
    public ClassType2 ClassTypeId2 { get; set; }
    public bool Confirmed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Class1 Class1 { get; set; }
    public int MatchTime => Class1?.Seconds ?? 0;

    // Fields That are valid only after projecting the incidents.
    public ErrorCode? ErrorCode1;
    public Data Data;

    public bool Condition1 => ErrorCode1 == ErrorCode.Test1;

    public bool Condition2 => ErrorCode1 == ErrorCode.Test1;

    public bool Condition3 => ErrorCode1 == ErrorCode.Test1;

    public static List<CapacityClass> GetList(int amount)
    {
        var list = new List<CapacityClass>();
        for (int i = 0; i < amount; i++)
        {
            list.Add(new CapacityClass
            {
                Id = i,
                Canceled = i % 2 == 0,
                CreatedAt = DateTime.Now,
                Class1 = new Class1
                {
                    Confirmed = i % 2 == 0,
                    Index = i,
                    Seconds = i
                }
            });
        }

        return list;
    }
}

public enum ClassType
{
    Test1, Test2, Test3, Test4, Test5, Test6
}

public enum ClassType2
{
    Test1, Test2, Test3, Test4, Test5, Test6
}

public class Class1
{
    public int Seconds { get; set; }
    public int Index { get; set; }
    public bool Confirmed { get; set; }
}

public enum ErrorCode
{
    Test1, Test2, Test3, Test4, Test5, Test6
}

public class Data
{
    public int[] Array1 { get; set; }
    public int[] Array2 { get; set; }
    public int[] Array3 { get; set; }
    public int Int1 { get; set; }
    public int Int2 { get; set; }
}