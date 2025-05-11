using MongoPlayground.Models;

namespace MongoPlayground.Repositories;

public interface IItemRepository
{
    Task<Item?> GetItemAsync(int id);
    Task<List<Item>> GetAllItemsAsync();
    Task<List<Item>> SearchItemsAsync(string name);
    Task AddItemAsync(Item item);
    Task UpdateItemAsync(Item item);
}
