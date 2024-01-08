namespace FeatureExamples;

//One task --> may be executed by many threads 
//One thread --> executes many tasks
public class AsyncExample
{
    public void Print()
    {
        Thread.Sleep(1000);
        Console.WriteLine("Hello world");
    }

    public async Task PrintAsync()
    {
        Task.Delay(10000);
        Console.WriteLine("Hello world");
        await Task.Delay(1000);
    }

    //this will crash the application
    //void does not create a state machine and so the exception is not captured
    public async static void BoomAsync()
    {
        await Task.Delay(1);
        throw new Exception("Boom");
    }

    public static async Task PrintAsyncOne()
    {
        Console.WriteLine(Environment.CurrentManagedThreadId);
        await Task.Delay(1000);
        Console.WriteLine("Hello from one");
        Console.WriteLine(Environment.CurrentManagedThreadId);
    }

    public static async Task PrintAsyncTwo()
    {
        await Task.Delay(2000);
        Console.WriteLine("Hello from two");
    }

    public static Task<int> DoSomethingAsync()
    {
        // This method does not use async-await but returns a Task.
        // Any exception thrown here will be captured in the returned Task.
        throw new InvalidOperationException("Something went wrong");
    }

    public async Task<int> CallDoSomethingAsync()
    {
        try
        {
            // Even though DoSomethingAsync doesn't use async-await,
            // awaiting its Task will still asynchronously wait for it to complete.
            return await DoSomethingAsync();
        }
        catch (InvalidOperationException ex)
        {
            // The exception thrown in DoSomethingAsync is caught here.
            Console.WriteLine(ex.Message);
            return -1;
        }
    }

}
