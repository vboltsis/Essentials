using System.Text.Json;

namespace InterviewExample;

public class OccurencesOfLetters
{
    public static Dictionary<char,int> GetOccurences(string text)
    {
        var dictionary = new Dictionary<char, int>();

        foreach (var c in text)
        {
            if (dictionary.TryGetValue(c, out var value))
            {
                dictionary[c] = value + 1;
            }
            else
            {
                dictionary.Add(c, 1);
            }
        }

        Console.WriteLine(JsonSerializer.Serialize(dictionary.ToList()));
        return dictionary;
    }
}
