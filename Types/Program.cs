using FeatureExamples;

string input = "12,34,56,78,90";

Span<int> numbers = stackalloc int[10];

var count = StackallocExample.ParseNumbers(input, numbers);

Console.WriteLine("Parsed Numbers:");
for (int i = 0; i < count; i++)
{
    Console.WriteLine(numbers[i]);
}