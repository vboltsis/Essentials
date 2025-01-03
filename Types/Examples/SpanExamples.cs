﻿namespace Types;

public class SpanExamples
{
    //monday:2020/1/1
    public static DateTime ParseDateFromStringSpan(string text)
    {
        var span = text.AsSpan();

        ReadOnlySpan<char> day = span.Slice(span.IndexOf(':') + 1);

        DateTime date = DateTime.Parse(day);

        return date;
    }

    public static DateTime ParseDateFromString(string text)
    {
        var parts = text.Split(':');

        var date = DateTime.Parse(parts[1]);

        return date;
    }

    //the input is of the form "123:text"
    public static int ExtractNumberWithSpan(string text)
    {
        var span = text.AsSpan();
        ReadOnlySpan<char> number = span[..span.IndexOf(':')];
        return int.Parse(number);
    }
}