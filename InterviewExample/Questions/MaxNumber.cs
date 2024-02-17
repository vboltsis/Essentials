namespace InterviewExample;

/*
 Given a list of numbers, find the max number in the list that exists as many times as the number itself.
 */

public class MaxNumber
{
    private static List<int> _numbers = new List<int> { 1, 2, 2, 3, 3, 3, 4, 5, 5, 5, 5, 5, 6, 7, 7, 7, 7, 7, 7 };

    public static int GetMaxDictionary()
    {
        var dictionary = new Dictionary<int, int>();

        foreach (var number in _numbers)
        {
            if (dictionary.TryGetValue(number, out int value))
            {
                dictionary[number] = ++value;
            }
            else
            {
                dictionary.Add(number, 1);
            }
        }

        dictionary = dictionary.Where(x => x.Key == x.Value).ToDictionary(x => x.Key, x => x.Value);

        return dictionary.MaxBy(x => x.Key).Key;
    }

    public static int GetMaxNumber()
    {
        var maxNumber = 0;
        var maxCount = 0;

        foreach (var number in _numbers)
        {
            var count = _numbers.Count(n => n == number);

            if (count > maxCount && count == number)
            {
                maxCount = count;
                maxNumber = number;
            }
        }

        return maxNumber;
    }

}
