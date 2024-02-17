using Newtonsoft.Json;

namespace Benchmark;

/*
| Method                    | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |----------:|----------:|----------:|-------:|-------:|----------:|
| SystemTextJsonSerialize   | 193.28 ns |  2.297 ns |  2.148 ns | 0.0172 |      - |     144 B |
| SystemTextJsonDeserialize | 289.19 ns |  0.857 ns |  0.760 ns | 0.0172 |      - |     144 B |
| NewtonsoftJsonSerialize   | 351.94 ns |  6.906 ns |  6.782 ns | 0.1740 |      - |    1456 B |
| NewtonsoftJsonDeserialize | 532.24 ns | 10.538 ns | 10.349 ns | 0.3290 | 0.0029 |    2752 B |
| SpanJsonSerialize         | 121.79 ns |  0.834 ns |  0.739 ns | 0.0172 |      - |     144 B |
| SpanJsonDeserialize       |  89.37 ns |  0.950 ns |  0.842 ns | 0.0172 |      - |     144 B |
 */

[MemoryDiagnoser]
public class SystemJsonVsNewtonsoft
{
    private static readonly Person person = new Person
    {
        Name = "John Doe",
        Age = 30,
        Email = "john.doe@example.com"
    };

    private static readonly string jsonString = "{\"Name\":\"John Doe\",\"Age\":30,\"Email\":\"john.doe@example.com\"}";

    [Benchmark]
    public string SystemTextJsonSerialize() => System.Text.Json.JsonSerializer.Serialize(person);

    [Benchmark]
    public Person SystemTextJsonDeserialize() => System.Text.Json.JsonSerializer.Deserialize<Person>(jsonString);

    [Benchmark]
    public string NewtonsoftJsonSerialize() => JsonConvert.SerializeObject(person);

    [Benchmark]
    public Person NewtonsoftJsonDeserialize() => JsonConvert.DeserializeObject<Person>(jsonString);

    [Benchmark]
    public string SpanJsonSerialize() => SpanJson.JsonSerializer.Generic.Utf16.Serialize(person);

    [Benchmark]
    public Person SpanJsonDeserialize() => SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Person>(jsonString);
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}