namespace FeatureExamples;

public class RandomExample
{
    private static readonly Random random = new Random();

    public static void TestRandom()
    {
        //Parallel.For(0, 10, i =>
        //{
        //    var random = Random.Shared;
        //    Console.WriteLine(random.Next(1, 100));

        //    //print current thread id
        //    Console.WriteLine("Thread Id: " + Thread.CurrentThread.ManagedThreadId);
        //});

        Parallel.For(0, 10_000, i =>
        {
            int randomNumber = random.Next(1, 100);
            Console.WriteLine($"Random number: {randomNumber}");
        });
    }

}
