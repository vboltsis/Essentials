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
        await Task.Delay(1000);
        Console.WriteLine("Hello world");
    }

    //this will crash the application
    public async static void BoomAsync()
    {
        try
        {
            await Task.Delay(1);
            throw new Exception("Boom");
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static async Task PrintAsyncOne()
    {
        Console.WriteLine(Environment.CurrentManagedThreadId);
        await Task.Delay(100);
        Console.WriteLine("Hello from one");
        Console.WriteLine(Environment.CurrentManagedThreadId);
    }

    public static async Task PrintAsyncTwo()
    {
        await Task.Delay(10);
        Console.WriteLine("Hello from two");
    }
}
