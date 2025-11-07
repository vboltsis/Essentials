namespace FeatureExamples;

public class StackallocExample
{
    public static void Example()
    {
        Span<int> array = stackalloc int[5];
    }
    
    public static int ParseNumbers(ReadOnlySpan<char> input, Span<int> output)
    {
        var count = 0;
        int currentNumber = 0;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c == ',')
            {
                if (count >= output.Length)
                    throw new ArgumentException("Output span is too small.");

                output[count++] = currentNumber;
                currentNumber = 0;
            }
            else if (char.IsDigit(c))
            {
                currentNumber = currentNumber * 10 + (c - '0');
            }
            else
            {
                throw new FormatException($"Invalid character '{c}' in input.");
            }
        }

        if (count >= output.Length)
            throw new ArgumentException("Output span is too small.");

        output[count++] = currentNumber;

        return count;
    }
}

