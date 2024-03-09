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
        var person = new Human("John", "Doe");
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
        ReadOnlySpan<char> output;

        if (format.IsEmpty || format == "F")
        {
            output = $"{FirstName} {LastName}".AsSpan();
        }
        else if (format == "I")
        {
            output = $"{FirstName[0]}.{LastName[0]}.".AsSpan();
        }
        else
        {
            throw new FormatException($"The format of '{format}' is invalid.");
        }

        if (destination.Length < output.Length)
        {
            charsWritten = 0;
            return false;
        }

        output.CopyTo(destination);
        charsWritten = output.Length;
        return true;
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