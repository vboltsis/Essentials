namespace InterviewExample.Questions;

public class FindDuplicateNumbersInTextFile {
    public static void FindDuplicates() {
        var numbers = new List<int>(16_000);
        var directory = Directory.GetCurrentDirectory();

        using (var reader = new StreamReader($"{directory}/numbers.txt")) {
            string line;
            while ((line = reader.ReadLine()) != null) {
                int number = int.Parse(line);
                numbers.Add(number);
            }
        }

        var duplicates = new List<int>();
        for (int i = 0; i < numbers.Count; i++) {
            for (int j = i + 1; j < numbers.Count; j++) {
                if (numbers[i] == numbers[j]) {
                    duplicates.Add(numbers[i]);
                    break;
                }
            }
        }

        Console.WriteLine("Duplicates:");
        foreach (int number in duplicates) {
            Console.WriteLine(number);
        }
    }
}
