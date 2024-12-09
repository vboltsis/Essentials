namespace Preview;

internal class TaskWhenEachExample
{
    public static async Task Example()
    {
        List<Task<int>> tasks = Enumerable.Range(1, 100).Select(Calculate).ToList();

        await foreach (var task in Task.WhenEach(tasks))
        {
            Console.WriteLine(await task);
        }

        async Task<int> Calculate(int order)
        {
            try
            {
                var number = Random.Shared.Next(1, 11);
                if (number > 5)
                {
                    throw new Exception("Hello PAM");
                }
            
                await Task.Delay(number);
                return order;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -99;
            }
        }
    }
}
