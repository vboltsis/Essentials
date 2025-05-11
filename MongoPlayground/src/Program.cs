// Program.cs – Minimal-API version
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoPlayground.Models;
using MongoPlayground.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

var conn = Environment.GetEnvironmentVariable("MONGO_CONN") ?? "mongodb://localhost:27017";
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(conn));

builder.Services.AddSingleton<IItemRepository, ItemRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IItemRepository>();

    if ((await repo.GetAllItemsAsync()).Count == 0)
    {
        var seed = new[]
        {
            new Item { Id = 1, Name = "Widget", Description = "A standard widget" },
            new Item { Id = 2, Name = "Gadget", Description = "A fancy gadget" }
        };
        foreach (var i in seed)
            await repo.AddItemAsync(i);
    }
}

app.MapGet("/items", async (IItemRepository repo)
        => Results.Ok(await repo.GetAllItemsAsync()));

app.MapGet("/items/{id:int}", async (int id, IItemRepository repo)
        => (await repo.GetItemAsync(id)) is { } item
            ? Results.Ok(item)
            : Results.NotFound());

app.MapGet("/search", async (string name, IItemRepository repo)
        => Results.Ok(await repo.SearchItemsAsync(name)));

app.MapPost("/items", async (Item item, IItemRepository repo) =>
{
    await repo.AddItemAsync(item);
    return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id:int}", async (int id, Item update, IItemRepository repo) =>
{
    if (id != update.Id)
        return Results.BadRequest("ID in route and body must match.");

    await repo.UpdateItemAsync(update);
    return Results.NoContent();
});

app.Run();
