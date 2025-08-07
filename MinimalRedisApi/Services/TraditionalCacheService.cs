using Microsoft.Extensions.Caching.Memory;
using MinimalRedisApi.Models;
using MinimalRedisApi.Repositories;

namespace MinimalRedisApi.Services;

public class TraditionalCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ProductRepository _repository;

    public TraditionalCacheService(IMemoryCache memoryCache, ProductRepository repository)
    {
        _memoryCache = memoryCache;
        _repository = repository;
    }

    public async Task<Product?> GetProductAsync(int productId)
    {
        var cacheKey = $"product_{productId}";
        
        if (_memoryCache.TryGetValue(cacheKey, out Product? cachedProduct))
        {
            return cachedProduct;
        }

        // Simulate cache stampede - multiple requests could hit the database simultaneously
        var product = await _repository.GetProductFromDbAsync(productId);
        
        if (product != null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            
            _memoryCache.Set(cacheKey, product, cacheEntryOptions);
        }

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        const string cacheKey = "all_products";
        
        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Product>? cachedProducts))
        {
            return cachedProducts!;
        }

        var products = await _repository.GetAllProductsAsync();
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        
        _memoryCache.Set(cacheKey, products, cacheEntryOptions);
        
        return products;
    }
} 