using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewExample.Questions;

//https://stackoverflow.com/questions/26798845/await-task-delay-vs-task-delay-wait

public class AsyncPrintQuestion
{
    /*
The TestWait method starts a new task using Task.Factory.StartNew.
Inside the task, it writes "Start" to the console.
Then it creates a delay task with Task.Delay(5000) which waits for 5000 milliseconds (5 seconds),
and then it explicitly waits for this delay to complete with .Wait().
After the delay, "Done" is written to the console.
The t.Wait() call outside of the task makes the main thread wait until the task t is completed.
Once the task is finished, "All done" is printed to the console.
     */

    /// <summary>
    /// prints start, wait five seconds and then all done
    /// </summary>
    public void TestWait()
    {
        var t = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Start");
            Task.Delay(5000).Wait();
            Console.WriteLine("Done");
        });
        t.Wait();
        Console.WriteLine("All done");
    }

    /*
The TestAwait method starts a new task that is intended to be asynchronous because of the async keyword.
However, Task.Factory.StartNew does not properly understand async delegates and will return a Task<Task> in this case.
It prints "Start" to the console.
It then awaits a 5000-millisecond delay.
You might expect "Done" to be printed after the delay, but there's a catch: the t.Wait() is called on the outer Task returned by Task.Factory.StartNew, not the inner task created by the async lambda. This means that t.Wait() completes as soon as the asynchronous operation is started, not when it is finished.
The main thread does not wait for the 5-second delay to complete and immediately prints "All done". 
    */

    /// <summary>
    /// prints start and instantly all done
    /// </summary>
    public void TestAwait()
    {
        var t = Task.Factory.StartNew(async () =>
        {
            Console.WriteLine("Start");
            await Task.Delay(5000);
            Console.WriteLine("Done");
        });
        t.Wait();
        Console.WriteLine("All done");
    }

    public void TestAwait2()
    {
        var t = Task.Run(async () =>
        {
            Console.WriteLine("Start");
            await Task.Delay(5000);
            Console.WriteLine("Done");
        });
        t.Wait();
        Console.WriteLine("All done");
    }
}
