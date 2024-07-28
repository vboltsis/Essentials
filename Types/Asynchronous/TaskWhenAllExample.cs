using System.Diagnostics;

namespace FeatureExamples;

internal class TaskWhenAllExample
{
    public async Task RunAsync()
    {
        var sw = Stopwatch.GetTimestamp();
        //await Wait1();

        //Console.WriteLine(Stopwatch.GetElapsedTime(sw).TotalMilliseconds);

        //await Wait2();

        //Console.WriteLine(Stopwatch.GetElapsedTime(sw).TotalMilliseconds);

        //await Wait5();

        await Task.WhenAll(Wait1(), Wait2(), Wait5());

        Console.WriteLine(Stopwatch.GetElapsedTime(sw).TotalMilliseconds);

        Console.WriteLine();

        async Task Wait1()
        {
            await Task.Delay(1000);
        }

        async Task Wait2()
        {
            await Task.Delay(2000);
        }

        async Task Wait5()
        {
            await Task.Delay(5000);
        }
    }
}
