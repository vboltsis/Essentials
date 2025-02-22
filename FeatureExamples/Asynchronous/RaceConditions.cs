using System.Runtime.CompilerServices;

namespace Types;

public class RaceConditions
{
    public static void IncreaseNumber()
    {
        var number = 0;

        for (int i = 0; i < 10_000; i++)
        {
            #region spoilers
            //fire and forget async
            //tasks are not run in the same order as created
            //we do not block any thread
            #endregion
            
            Task.Run(() => 
            {
                //db call
                number++;
            });
        }
        
        Thread.Sleep(1000);

        Console.WriteLine(number);
    }
    
    public static void IncreaseNumberSolution1()
    {
        var number = 0;

        for (int i = 0; i < 10_000; i++)
        {
            Task.Run(() => 
            {
                //db call
                Interlocked.Increment(ref number);
            });
        }

        Thread.Sleep(1000);
        Console.WriteLine(number);
    }
    
    public static async Task IncreaseNumberSolution2()
    {
        var number = 0;
        var lockObject = new object();

        for (int i = 0; i < 10_000; i++)
        {
            Task.Run(() => 
            {
                //db call
                lock (lockObject)
                {
                    number++;
                }
            });
        }

        await Task.Delay(1000);
        Console.WriteLine(number);
    }
    
    public static async Task IncreaseNumberSolution3()
    {
        for (int i = 0; i < 10_000; i++)
        {
            Task.Run(() => 
            {
                //db call
                IncreaseNumber(Test);
            });
        }

        await Task.Delay(1000);
        Console.WriteLine(Test);
    }

    public static volatile int Test;

    [MethodImpl(MethodImplOptions.Synchronized)]
    static void IncreaseNumber(int number)
    {
        Test++;
    }
}




/*When it makes sense to use await Task.Run()?
1) Legacy Libraries: You might be dealing with a legacy synchronous library or method that you can't modify.
If this library has a CPU-bound operation that takes a long time,
offloading that work can prevent that operation from holding up other tasks that could be processed on the main thread.

2)Throttling Resources: In rare scenarios where you have a limited resource and many incoming requests,
you might use Task.Run to artificially queue up processing to limit the number of concurrent operations against that resource.
It's not a common pattern, and there are often better ways to manage limited resources, but it can be used as a temporary measure.

3) ASP.NET Core SignalR: SignalR hubs have a specific context, and sometimes,
for various reasons (like avoiding deadlocks or certain race conditions), you might want to move certain operations off the hub thread.

4) Offloading to avoid timeout: In scenarios where there's a potential for a web request to timeout due to some lengthy CPU-bound operation
(which isn't the norm in I/O-bound web servers), moving the operation to a background thread could make sense.
However, this is more of a stop-gap than a recommended practice. Ideally, such operations might be better handled by background processing frameworks or out-of-process tasks.
*/