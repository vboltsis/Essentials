using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Concurrency;
using System.Diagnostics;

// Configure the host
using var host = new HostBuilder()
    .UseOrleans(builder => builder.UseLocalhostClustering())
    .Build();

// Start the host
await host.StartAsync();

// Get the grain factory
var grainFactory = host.Services.GetRequiredService<IGrainFactory>();

// Get a reference to the HelloGrain grain with the key "friend"
var friendGrain = grainFactory.GetGrain<IHelloGrain>("friend");

// Call the grain and print the result to the console
var result = friendGrain.SayHello("Good morning!");
var d = result.IsCompletedSuccessfully;

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

    public ValueTask<string> SayHello(string greeting)
    {
        Console.WriteLine("Start " + DateTime.Now);
        var d = Stopwatch.StartNew();
        while (d.ElapsedMilliseconds < 5000)
        {

        }
        _greeting = greeting;
        Console.WriteLine("Finished Hello " + DateTime.Now);
        return ValueTask.FromResult($"Hello, {greeting}!");
    }

    public ValueTask<string> GetGreeting()
    {
        Console.WriteLine("Retrieved Greetings "+ DateTime.Now);
        return ValueTask.FromResult(_greeting);
    }
}

public interface IHelloGrain : IGrainWithStringKey
{
    ValueTask<string> SayHello(string greeting);
    [AlwaysInterleave]
    ValueTask<string> GetGreeting();
}