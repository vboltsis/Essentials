using System.Text.Json;

namespace InterviewExample;

public class OccurrencesOfLetters
{
    public static Dictionary<char,int> GetOccurrences(string text)
    {
        var dictionary = new Dictionary<char, int>();

        foreach (var character in text)
        {
            if (dictionary.TryGetValue(character, out var value))
            {
                dictionary[character] = value + 1;
            }
            else
            {
                dictionary.Add(character, 1);
            }
        }

        Console.WriteLine(JsonSerializer.Serialize(dictionary.ToList()));
        return dictionary;
    }
}
