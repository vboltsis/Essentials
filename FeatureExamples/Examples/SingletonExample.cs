using System.Text.Json;

namespace FeatureExamples;

public sealed class Singleton1
{
    private static Singleton1 _instance;
    private static readonly object _lock = new object();

    private Singleton1() { }

    public static Singleton1 GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Singleton1();
                }
            }
        }

        return _instance;
    }

    public static void Print()
    {
        Console.WriteLine("Hello world");
    }
}

public sealed class Singleton2
{
    private static readonly Singleton2 _instance = new Singleton2();

    private Singleton2() { }

    public static Singleton2 GetInstance()
    {
        return _instance;
    }
}

public class JsonSerializerSettings
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
    
    private JsonSerializerSettings()
    {
    }

    public static JsonSerializerOptions Options
    {
        get { return _options; }
    }
}
