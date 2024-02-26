namespace Benchmark.Classes;

/*
|              Method |     Mean |   Error |   StdDev | Allocated |
|-------------------- |---------:|--------:|---------:|----------:|
|      GetWeatherTask | 284.1 ms | 5.59 ms | 10.37 ms |  45.82 KB |
| GetWeatherValueTask | 284.0 ms | 5.63 ms |  8.76 ms |  45.84 KB |
 */

[MemoryDiagnoser]
public class TaskVsValueTaskBad
{
    [Benchmark]
    public async Task<string> GetWeatherTask()
    {
       return await Helper.GetWeatherAsync();
    }

    [Benchmark]
    public async ValueTask<string> GetWeatherValueTask()
    {
        return await Helper.GetWeatherValueAsync();
    }
}

//THIS CHECKS THE PERFORMANCE WHEN USING THE VALUE TASK THE WRONG WAY
internal static class Helper
{
    public static async Task<string> GetWeatherAsync()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=Athens"),
            Headers =
            {
                { "X-RapidAPI-Key", "KEY" },
                { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
            },
        };
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }

    public static async ValueTask<string> GetWeatherValueAsync()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=Athens"),
            Headers =
            {
                { "X-RapidAPI-Key", "KEY" },
                { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
            },
        };
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
}