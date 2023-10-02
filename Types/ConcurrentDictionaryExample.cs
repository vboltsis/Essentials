using System.Collections.Concurrent;

namespace FeatureExamples;

public class FactorialService
{
    private ConcurrentDictionary<int, long> _cache = new(Environment.ProcessorCount * 2, 11);

    public long ComputeFactorial(int number)
    {
        if (number < 0)
        {
            throw new ArgumentException("Number should be non-negative.");
        }

        // If the result is already in the cache, return it.
        if (_cache.TryGetValue(number, out long cachedResult))
        {
            Console.WriteLine($"Cache hit for {number}!");
            return cachedResult;
        }

        // Compute the factorial.
        long result = 1;
        for (int i = 1; i <= number; i++)
        {
            result *= i;
        }

        // Cache the result.
        _cache[number] = result;

        return result;
    }

    public static void Compute()
    {
        var service = new FactorialService();

        Console.WriteLine(service.ComputeFactorial(5));  // Computes and caches the result.
        Console.WriteLine(service.ComputeFactorial(5));  // Uses cached result.
        Console.WriteLine(service.ComputeFactorial(7));  // Computes and caches the result.
    }
}
