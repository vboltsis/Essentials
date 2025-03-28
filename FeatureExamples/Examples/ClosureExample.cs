namespace FeatureExamples;

internal class ClosureExample
{
    public static void BadExample()
    {
        for (int i = 0; i < 5; i++)
        {
            Task.Run(() => Console.WriteLine(i));
        }
    }

    public static void GoodExample()
    {
        for (int i = 0; i < 5; i++)
        {
            int j = i;
            Task.Run(() => Console.WriteLine(j));
        }
    }

    public static void LinqExample()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        int multiplier = 1;
        var query = numbers.Select(n => n * multiplier);

        multiplier = 2;
        var resultList = query.ToList(); // You might expect [1, 2, 3, 4, 5, 6]

        Console.WriteLine(string.Join(", ", resultList));
        // Actual output: 2, 4, 6, 8, 10, 12
    }
}
