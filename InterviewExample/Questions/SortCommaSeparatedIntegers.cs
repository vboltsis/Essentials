namespace InterviewExample.Questions;

internal class SortCommaSeparatedIntegers
{
    public static string Sort(string numbers)
    {
        return string.Join(",", numbers.Split(',').Select(c => Convert.ToInt32(c)).OrderBy(i => i));
    }

    public static string SortWithoutLinq(string numbers)
    {
        var numbersArray = numbers.Split(',');
        var numbersInt = new int[numbersArray.Length];
        for (int i = 0; i < numbersArray.Length; i++)
        {
            numbersInt[i] = Convert.ToInt32(numbersArray[i]);
        }

        Array.Sort(numbersInt);

        return string.Join(",", numbersInt);
    }
}
