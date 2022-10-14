using BenchmarkDotNet.Attributes;

namespace Benchmark.Classes;

/*
|             Method |     Mean |    Error |   StdDev |   Gen0 | Allocated |
|------------------- |---------:|---------:|---------:|-------:|----------:|
| GetPersonAddresses | 16.37 ns | 0.124 ns | 0.116 ns | 0.0162 |     136 B |
|           GetTuple | 27.83 ns | 0.094 ns | 0.078 ns | 0.0162 |     136 B |
 */

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

    [Benchmark]
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