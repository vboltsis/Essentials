using MinimalRedisApi.Models;

namespace MinimalRedisApi.Repositories;

public class ProductRepository
{
    private readonly Dictionary<int, Product> _products = new()
    {
        { 1, new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, CreatedAt = DateTime.UtcNow.AddDays(-30), IsActive = true } },
        { 2, new Product { Id = 2, Name = "Mouse", Description = "Wireless gaming mouse", Price = 49.99m, CreatedAt = DateTime.UtcNow.AddDays(-15), IsActive = true } },
        { 3, new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 129.99m, CreatedAt = DateTime.UtcNow.AddDays(-10), IsActive = true } },
        { 4, new Product { Id = 4, Name = "Monitor", Description = "27-inch 4K monitor", Price = 299.99m, CreatedAt = DateTime.UtcNow.AddDays(-5), IsActive = true } },
        { 5, new Product { Id = 5, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 199.99m, CreatedAt = DateTime.UtcNow.AddDays(-1), IsActive = true } }
    };

    public async Task<Product?> GetProductFromDbAsync(int productId)
    {
        // Simulate database latency
        await Task.Delay(100);

        return _products.TryGetValue(productId, out var product) ? product : null;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        // Simulate database latency
        await Task.Delay(200);
        
        return _products.Values;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Simulate database latency
        await Task.Delay(150);
        
        product.Id = _products.Max(x => x.Key) + 1;
        product.CreatedAt = DateTime.UtcNow;
        _products[product.Id] = product;
        
        return product;
    }
} 