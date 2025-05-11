using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoPlayground.Models;

namespace MongoPlayground.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly IMongoCollection<Item> _itemsCollection;
    private readonly IMongoCollection<SearchEntry> _searchCollection;
    private readonly IMemoryCache _cache;

    public ItemRepository(IMongoClient mongoClient, IMemoryCache cache)
    {
        var database = mongoClient.GetDatabase("DemoDb");
        _itemsCollection = database.GetCollection<Item>("Items");
        _searchCollection = database.GetCollection<SearchEntry>("ItemSearchIndex");
        _cache = cache;
    }

    public async Task<Item?> GetItemAsync(int id)
    {
        var key = $"Item:{id}";
        if (_cache.TryGetValue(key, out Item cached))
        {
            return cached;
        }

        var item = await _itemsCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        if (item is not null)
        {
            _cache.Set(key, item);
        }

        return item;
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _itemsCollection.Find(FilterDefinition<Item>.Empty).ToListAsync();
    }

    public async Task AddItemAsync(Item item)
    {
        await _itemsCollection.InsertOneAsync(item);

        var entry = new SearchEntry { Id = item.Id, Name = item.Name };
        await _searchCollection.InsertOneAsync(entry);
    }

    public async Task UpdateItemAsync(Item item)
    {
        await _itemsCollection.ReplaceOneAsync(i => i.Id == item.Id, item);

        var entry = new SearchEntry { Id = item.Id, Name = item.Name };
        await _searchCollection.ReplaceOneAsync(
            Builders<SearchEntry>.Filter.Eq(e => e.Id, item.Id),
            entry,
            new ReplaceOptions { IsUpsert = true }
        );

        _cache.Remove($"Item:{item.Id}");
    }

    public async Task<List<Item>> SearchItemsAsync(string name)
    {
        var filter = Builders<SearchEntry>.Filter.Regex(e => e.Name, new BsonRegularExpression(name, "i"));
        var matches = await _searchCollection.Find(filter).ToListAsync();

        var ids = matches.Select(m => m.Id).ToList();

        var itemsFilter = Builders<Item>.Filter.In(i => i.Id, ids);
        var items = await _itemsCollection.Find(itemsFilter).ToListAsync();

        foreach (var item in items)
        {
            _cache.Set($"Item:{item.Id}", item);
        }

        return items;
    }
}
