namespace InterviewExample.Questions;

public class PalindromeString
{
    public static bool IsPalindrome(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        var left = 0;
        var right = text.Length - 1;

        while (left < right)
        {
            if (text[left] != text[right])
            {
                return false;
            }

            left++;
            right--;
        }

        return true;
    }

    public static bool IsPalindromeRecursive(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        if (text.Length == 1)
        {
            return true;
        }

        if (text[0] != text[text.Length - 1])
        {
            return false;
        }

        return IsPalindromeRecursive(text.Substring(1, text.Length - 2));
    }

    public static bool IsPalindromeLinq(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }

        return text.SequenceEqual(text.Reverse());
    }
}
