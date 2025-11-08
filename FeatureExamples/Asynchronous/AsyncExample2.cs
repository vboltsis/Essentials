namespace FeatureExamples.Asynchronous;

public class AsyncExample2
{
    //1
    public static string GetHomepage(string url)
    {
        using var http = new HttpClient();
        return http.GetStringAsync(url).Result; // blocks; can deadlock
    }
    
    public static async Task<string> GetHomepageAsync(string url)
    {
        using var http = new HttpClient();
        return await http.GetStringAsync(url);
    }
    
    //2
    public static void StartWork()
    {
        DoWorkAsync(); // fire-and-forget; exceptions go unobserved
    }
    
    public static Task StartWorkAsync() => DoWorkAsync(); // caller can await/handle
    
    private static async Task DoWorkAsync()
    {
        await Task.Delay(100);
        throw new InvalidOperationException("Boom");
    }
    
    //3
    public static async void SaveAsync1(string path, string text)
    {
        await File.WriteAllTextAsync(path, text); // exceptions crash the process in some contexts
    }

    public static Task SaveAsync2(string path, string text)
        => File.WriteAllTextAsync(path, text);
    
    //4
    public static async Task<int> TotalLengthAsync1(IEnumerable<string> urls)
    {
        using var http = new HttpClient();
        var tasks = urls.Select(async u => (await http.GetStringAsync(u)).Length);
        // Oops: counting tasks, not results
        return tasks.Count();
    }
    
    public static async Task<int> TotalLengthAsync2(IEnumerable<string> urls)
    {
        using var http = new HttpClient();
        var tasks = urls.Select(async u => (await http.GetStringAsync(u)).Length);
        var lengths = await Task.WhenAll(tasks);
        return lengths.Sum();
    }

    //5
    public static async Task<string> DownloadAsync1(string url, CancellationToken ct)
    {
        using var http = new HttpClient();
        var resp = await http.GetAsync(url); // ct not used
        ct.ThrowIfCancellationRequested();   // too late
        return await resp.Content.ReadAsStringAsync(); // ct not used
    }
    
    public static async Task<string> DownloadAsync2(string url, CancellationToken ct)
    {
        using var http = new HttpClient();
        using var resp = await http.GetAsync(url, ct);
        return await resp.Content.ReadAsStringAsync(ct);
    }
    
    //6
    private static readonly SemaphoreSlim _gate = new(1, 1);
    private static int _count;

    public static async Task<int> IncrementAsync1()
    {
        await _gate.WaitAsync();
        await Task.Delay(10);
        _count++;
        _gate.Release(); // if exception thrown above, Release never happens (leak)
        return _count;
    }
    
    public static async Task<int> IncrementAsync2()
    {
        await _gate.WaitAsync();
        try
        {
            await Task.Delay(10);
            return ++_count;
        }
        finally
        {
            _gate.Release();
        }
    }

    //7
    public static Task<string> ReadFileAsync1(string path)
    {
        return Task.Run(() => File.ReadAllText(path)); // burns a thread
    }
    
    public static Task<string> ReadFileAsync2(string path)
    {
        return File.ReadAllTextAsync(path);
    }
    
    //8
    public static Task WriteTwoFilesAsync1(string a, string b, string content)
    {
        File.WriteAllTextAsync(a, content);
        File.WriteAllTextAsync(b, content);
        return Task.CompletedTask; // returns before writes finish
    }
    
    public static async Task WriteTwoFilesAsync2(string a, string b, string content)
    {
        var t1 = File.WriteAllTextAsync(a, content);
        var t2 = File.WriteAllTextAsync(b, content);
        await Task.WhenAll(t1, t2);
    }


}