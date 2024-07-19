using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Concurrency;
using Orleans.Configuration;
using Orleans.Runtime;

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

var customerGrain = grainFactory.GetGrain<ICustomerGrain>(420);

Task.Run(async () =>
{
    var result = await customerGrain.LongRunningMethod();
    Console.WriteLine($"Result is {result}");
});

await Task.Delay(1000);

var id = await customerGrain.GetId();

Console.WriteLine($"Id is {id}");

// // Get a reference to the HelloGrain grain with the key "friend"
// var friendGrain = grainFactory.GetGrain<IHelloGrain>("friend");
//
// // Call the grain and print the result to the console
// await Task.WhenAll(friendGrain.SayHello("Good morning!").AsTask(),
//     friendGrain.GetGreeting().AsTask(), friendGrain.GetSomething().AsTask());
//
// var greeting = await friendGrain.GetGreeting();

await Task.Delay(10_000);

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

public class CustomerGrain : Grain, ICustomerGrain
{
    public Customer Customer { get; set; }
    public long CustomerId { get; set; }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        CustomerId = this.GetPrimaryKeyLong();
        Customer = new Customer {Id = CustomerId, Name = "Techsson"};
        return base.OnActivateAsync(cancellationToken);
    }
    
    public ValueTask<long> GetId()
    {
        return ValueTask.FromResult(CustomerId);
    }

    public ValueTask<string> GetCustomerName()
    {
        return ValueTask.FromResult(Customer.Name);
    }

    public async ValueTask<bool> LongRunningMethod()
    {
        await Task.Delay(10_000);
        return true;
    }
}

public interface ICustomerGrain : IGrainWithIntegerKey
{
    [AlwaysInterleave]
    ValueTask<long> GetId();
    ValueTask<string> GetCustomerName();
    ValueTask<bool> LongRunningMethod();
}

public class Customer
{
    public long Id { get; set; }
    public string Name { get; set; }
}

//1. Uniqueness. Each grain has a unique key
//2. Solves Concurrency using a single thread for each grain instance
//3. We have the state available in memory