namespace Preview;

internal class TaskWhenEachExample
{
    public static async Task Example()
    {
        List<Task<int>> tasks = Enumerable.Range(1, 5).Select(Calculate).ToList();

        await foreach (var task in Task.WhenEach(tasks))
        {
            Console.WriteLine(await task);
        }

        async Task<int> Calculate(int order)
        {
            var number = Random.Shared.Next(1, 100);
            await Task.Delay(number);
            return order;
        }
    }
}
