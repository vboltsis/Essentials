namespace FeatureExamples;

//Extension methods allow you to add methods to existing types without creating a new derived type,
//recompiling, or otherwise modifying the original type.

public static class ExtensionMethodsExample
{
    public static int CountCharacter(this string str, char character)
    {
        var count = 0;

        foreach (var c in str)
        {
            if (c == character)
            {
                count++;
            }
        }

        return count;
    }

    public static int CountNumberOccurrences(this int[] array, int number)
    {
        var count = 0;

        foreach (var n in array)
        {
            if (n == number)
            {
                count++;
            }
        }

        return count;
    }
}
