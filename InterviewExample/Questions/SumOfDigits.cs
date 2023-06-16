namespace InterviewExample;

/*
You are given a positive integer, the method should return the sum of all digits of that integer
Example: 1234 -> 10
 */

public class SumOfDigits
{
    public static int GetSumOfDigits(int number)
    {
        var sum = 0;
        while (number > 0)
        {
            sum += number % 10;
            number /= 10;
        }

        return sum;
    }

    public static int GetSumOfDigitsRecursive(int number)
    {
        if (number == 0)
        {
            return 0;
        }

        return number % 10 + GetSumOfDigitsRecursive(number / 10);
    }

    public static int GetSumOfDigitsLinq(int number)
    {
        return number.ToString().Sum(c => c - '0');
    }

    public static int GetSumOfDigitsLinq2(int number)
    {
        return number.ToString().Select(c => int.Parse(c.ToString())).Sum();
    }
}
