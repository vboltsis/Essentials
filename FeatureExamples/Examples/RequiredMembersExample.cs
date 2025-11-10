#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace FeatureExamples;

/// <summary>
/// Shows how required members enforce initialization, and how constructors can satisfy them with SetsRequiredMembers.
/// </summary>
public static class RequiredMembersExample
{
    public static void Run()
    {
        var options = new ClientOptions
        {
            Endpoint = "https://api.example",
            ApiKey = "super-secret",
            Timeout = TimeSpan.FromSeconds(5),
            Region = "eu-west-1"
        };

        Console.WriteLine($"Configured endpoint: {options.Endpoint}");

        var generated = new ClientOptions("metrics");
        Console.WriteLine($"Generated endpoint: {generated.Endpoint}");

        // The following line would not compile because required members would be missing:
        // var missing = new ClientOptions();
    }

    public sealed class ClientOptions
    {
        public required string Endpoint { get; init; }
        public required string ApiKey { get; init; }
        public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
        public string? Region { get; init; }

        public ClientOptions()
        {
        }

        [SetsRequiredMembers]
        public ClientOptions(string serviceName)
        {
            Endpoint = $"https://{serviceName}.service";
            ApiKey = $"{serviceName}-generated-key";
        }
    }
}

