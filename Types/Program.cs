var numbers = new List<int> { 10, 5, 8, 3, 9 };
numbers.Sort(ReverseComparer.Instance);

foreach (var number in numbers)
{
    Console.WriteLine(number);
}

int[] array = { 10, 5, 8, 3, 9 };
Array.Sort(array);

foreach (var number in array)
{
    Console.WriteLine(number);
}

public class ReverseComparer : IComparer<int>
{
    private ReverseComparer() { }

    public static ReverseComparer Instance { get; } = new ReverseComparer();

    public int Compare(int x, int y)
    {
        return y.CompareTo(x); // Reverse the default sort order.
    }
}


