namespace FeatureExamples;

public class LazyExample
{
    private static Lazy<ExpensiveObject> lazyExpensiveObject = new(() => new ExpensiveObject());
    private static Lazy<ExpensiveObject> defaultLazy = new(() => new ExpensiveObject(), LazyThreadSafetyMode.ExecutionAndPublication);

    private static AsyncLazy<ExpensiveAsyncObject> asyncLazyExpensiveObject = new AsyncLazy<ExpensiveAsyncObject>(async () =>
    {
        var obj = new ExpensiveAsyncObject();
        await obj.InitializeAsync();
        return obj;
    });

    public static void Example()
    {
        Console.WriteLine("Program started.");
        var obj = lazyExpensiveObject.Value; // ExpensiveObject is created here
        Console.WriteLine("ExpensiveObject accessed.");
    }

    public static async Task ExampleAsync()
    {
        Console.WriteLine("Program started.");
        var obj = await asyncLazyExpensiveObject.Value; // ExpensiveAsyncObject is created here
        Console.WriteLine("ExpensiveAsyncObject accessed.");
    }
}

public class ExpensiveObject
{
    public ExpensiveObject()
    {
        Console.WriteLine("ExpensiveObject created.");
    }
}

public class ExpensiveAsyncObject
{
    public ExpensiveAsyncObject()
    {
        Console.WriteLine("ExpensiveAsyncObject created.");
    }

    public async Task InitializeAsync()
    {
        await Task.Delay(1000); // Simulate async initialization
        Console.WriteLine("ExpensiveAsyncObject initialized.");
    }
}

public class AsyncLazy<T>
{
    private readonly Lazy<Task<T>> instance;

    public AsyncLazy(Func<T> factory)
    {
        instance = new Lazy<Task<T>>(() => Task.Run(factory));
    }

    public AsyncLazy(Func<Task<T>> factory)
    {
        instance = new Lazy<Task<T>>(() => Task.Run(factory));
    }

    public Task<T> Value => instance.Value;
}
