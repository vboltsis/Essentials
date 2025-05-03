namespace FeatureExamples.Asynchronous;

public class DeadlockExample
{
    static object lockA = new object();
    static object lockB = new object();

    public static void Example()
    {
        new Thread(Worker1).Start();
        new Thread(Worker2).Start();
    }

    static void Worker1()
    {
        lock (lockA)
        {
            Console.WriteLine("Worker1: got A, now waiting for B...");
            Thread.Sleep(50);
            lock (lockB)
            {
                Console.WriteLine("Worker1: got B too!");
            }
        }
    }

    static void Worker2()
    {
        lock (lockB)
        {
            Console.WriteLine("Worker2: got B, now waiting for A...");
            Thread.Sleep(50);
            lock (lockA)
            {
                Console.WriteLine("Worker2: got A too!");
            }
        }
    }
}
