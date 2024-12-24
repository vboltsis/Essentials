namespace Benchmark;

[MemoryDiagnoser] 
public class ClassVsStruct
{
    [Benchmark]
    public StudentStructDto GetStruct()
    {
        return new StudentStructDto
        {
            Age = 10,
            Name = "Takis"
        };
    }

    [Benchmark]
    public StudentClass GetClass()
    {
        return new StudentClass
        {
            Age = 10,
            Name = "Takis"
        };
    }
}

public readonly struct StudentStructDto
{
    public int Age { get; init; }
    public string Name { get; init; }
}

public class StudentClass
{
    public int Age { get; set; }
    public string Name { get; set; }
}
