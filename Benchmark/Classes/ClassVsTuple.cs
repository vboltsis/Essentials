using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmark.Classes;

/*
|             Method |      Job |  Runtime |     Mean |    Error |   StdDev |   Gen0 | Allocated |
|------------------- |--------- |--------- |---------:|---------:|---------:|-------:|----------:|
| GetPersonAddresses | .NET 6.0 | .NET 6.0 | 17.10 ns | 0.394 ns | 0.368 ns | 0.0162 |     136 B |
|           GetTuple | .NET 6.0 | .NET 6.0 | 16.73 ns | 0.212 ns | 0.198 ns | 0.0162 |     136 B |
| GetPersonAddresses | .NET 7.0 | .NET 7.0 | 16.45 ns | 0.260 ns | 0.243 ns | 0.0162 |     136 B |
|           GetTuple | .NET 7.0 | .NET 7.0 | 16.10 ns | 0.202 ns | 0.189 ns | 0.0162 |     136 B |
 */

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public class ClassVsTuple
{
    [Benchmark]
    public PersonAddress[] GetPersonAddresses()
    {
        return new PersonAddress[]
        {
            new PersonAddress
            {
                Address = new Address
                {
                    Id = 1,
                    Name = "a",
                    StreetNumber = 1
                },
                Person = new Person
                {
                    Id = 2,
                    FirstName= "b",
                    LastName= "c"
                }
            }
        };
    }

    [Benchmark(Baseline = true)]
    public Tuple<Person, Address>[] GetTuple()
    {
        return new Tuple<Person, Address>[]
        {
            new Tuple<Person, Address>
            (
                new Person
                {
                    Id = 2,
                    FirstName= "b",
                    LastName= "c"
                },
                new Address
                {
                    Id = 1,
                    Name = "a",
                    StreetNumber = 1
                }
            )
        };
    }
}

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Address
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int StreetNumber { get; set; }
}

public class PersonAddress
{
    public Person Person { get; set; }
    public Address Address { get; set; }
}