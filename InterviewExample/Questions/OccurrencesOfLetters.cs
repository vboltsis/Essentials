using System.Text.Json;

namespace InterviewExample;

public class OccurrencesOfLetters
{
    /// <summary>
    /// Gets the occurrences of each letter in a string
    /// </summary>
    /// <param name="text">The text to check</param>
    /// <returns></returns>
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
