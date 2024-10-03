using System;

namespace AllDotNetInterfaces;

public static class SpanFormattableExample
{
    public static void Print()
    {
        double value = 1234.5678;
        Span<char> buffer = new char[20];

        if (value.TryFormat(buffer, out int charsWritten, "N2"))
        {
            Console.WriteLine(new string(buffer.Slice(0, charsWritten)));
        }
    }

    public static void PrintHuman()
    {
        // Usage:
        var person = new Human("Takis", "Anagnostaras");
        Span<char> buffer = new char[50];

        if (person.TryFormat(buffer, out int charsWritten, "F".AsSpan()))
        {
            Console.WriteLine(new string(buffer.Slice(0, charsWritten)));  // Outputs: John Doe
        }

        if (person.TryFormat(buffer, out charsWritten, "I".AsSpan()))
        {
            Console.WriteLine(new string(buffer.Slice(0, charsWritten)));  // Outputs: J.D.
        }
    }
}

public class Human : ISpanFormattable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Human(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
    {
        int requiredLength;
        if (format.IsEmpty || format.SequenceEqual("F".AsSpan()))
        {
            requiredLength = FirstName.Length + 1 + LastName.Length;
            if (destination.Length < requiredLength)
            {
                charsWritten = 0;
                return false;
            }

            FirstName.AsSpan().CopyTo(destination);
            destination[FirstName.Length] = ' ';
            LastName.AsSpan().CopyTo(destination.Slice(FirstName.Length + 1));

            charsWritten = requiredLength;
            return true;
        }
        else if (format.SequenceEqual("I".AsSpan()))
        {
            requiredLength = 4; // e.g., "J.D."
            if (destination.Length < requiredLength)
            {
                charsWritten = 0;
                return false;
            }

            // Format as "F.L."
            destination[0] = FirstName[0];
            destination[1] = '.';
            destination[2] = LastName[0];
            destination[3] = '.';

            charsWritten = requiredLength;
            return true;
        }
        else
        {
            //throw new FormatException($"The format of '{format.ToString()}' is invalid.");
            charsWritten = 0;
            return false;
        }
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format) || format == "F")
        {
            return $"{FirstName} {LastName}";
        }
        else if (format == "I")
        {
            return $"{FirstName[0]}.{LastName[0]}.";
        }
        else
        {
            throw new FormatException($"The format of '{format}' is invalid.");
        }
    }
}
