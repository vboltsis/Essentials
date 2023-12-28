using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Concurrency;
using Orleans.Configuration;

using var host = new HostBuilder()
    .UseOrleans(builder => builder.UseLocalhostClustering()
    .Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "dev";
        options.ServiceId = "HelloWorldApp";
    })
    .Configure<GrainCollectionOptions>(options =>
    {
        options.CollectionAge = TimeSpan.FromMinutes(2);
    }))
    .Build();

// Start the host
await host.StartAsync();

// Get the grain factory
var grainFactory = host.Services.GetRequiredService<IGrainFactory>();

// Get a reference to the HelloGrain grain with the key "friend"
var friendGrain = grainFactory.GetGrain<IHelloGrain>("friend");

// Call the grain and print the result to the console
await Task.WhenAll(friendGrain.SayHello("Good morning!").AsTask(),
    friendGrain.GetGreeting().AsTask(), friendGrain.GetSomething().AsTask());

var greeting = await friendGrain.GetGreeting();

Console.WriteLine("Orleans is running.\nPress Enter to terminate...");
Console.ReadLine();
Console.WriteLine("Orleans is stopping...");

await host.StopAsync();

public sealed class HelloGrain : Grain, IHelloGrain
{
    private string _greeting = "hello";

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Activating Friend Grain " + DateTime.Now);
        return base.OnActivateAsync(cancellationToken);
    }

    public async ValueTask<string> SayHello(string greeting)
    {
        Console.WriteLine("Start");

        await Task.Delay(5000);

        _greeting = greeting;
        Console.WriteLine($"Finished: {_greeting}");
        return $"Hello, {greeting}!";
    }

    public ValueTask<string> GetGreeting()
    {
        Console.WriteLine($"Got: {_greeting}");
        return ValueTask.FromResult(_greeting);
    }

    public ValueTask<string> GetSomething()
    {
        Console.WriteLine("Something");
        return ValueTask.FromResult("Something");
    }
}

public interface IHelloGrain : IGrainWithStringKey
{
    ValueTask<string> SayHello(string greeting);
    [AlwaysInterleave]
    ValueTask<string> GetGreeting();
    ValueTask<string> GetSomething();
}