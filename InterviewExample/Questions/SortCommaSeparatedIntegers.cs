namespace InterviewExample.Questions;

internal class SortCommaSeparatedIntegers
{
    public static string Sort(string numbers)
    {
        return string.Join(",", numbers.Split(',').Select(c => Convert.ToInt32(c)).OrderBy(i => i));
    }
}
