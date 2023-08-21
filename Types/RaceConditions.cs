namespace Types;

public class RaceConditions
{
    public static void IncreaseNumber()
    {
        var number = 0;

        for (int i = 0; i < 10_000; i++)
        {
            //fire and forget async
            //tasks are not run in the same order as created
            //we do not block any thread

            Task.Run(() => 
            {
                Console.WriteLine("Hello from inside");
                number++;
            });

            Console.WriteLine("Test");
        }

        Console.WriteLine(number);
    }

}
