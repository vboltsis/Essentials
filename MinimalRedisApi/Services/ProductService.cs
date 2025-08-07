using Microsoft.Extensions.Caching.Hybrid;
using MinimalRedisApi.Models;
using MinimalRedisApi.Repositories;

namespace MinimalRedisApi.Services;

public class ProductService
{
    private readonly HybridCache _cache;
    private readonly ProductRepository _repository;

    public ProductService(HybridCache cache, ProductRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<Product?> GetProductAsync(int productId)
    {
        return await _cache.GetOrCreateAsync(
            $"product_{productId}",
            async (cancellationToken) =>
            {
                // This block is executed only by a single request
                return await _repository.GetProductFromDbAsync(productId);
            });
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _cache.GetOrCreateAsync(
            "all_products",
            async (cancellationToken) =>
            {
                return await _repository.GetAllProductsAsync();
            });
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        var createdProduct = await _repository.CreateProductAsync(product);
        
        // Invalidate cache after creating a new product
        await _cache.RemoveAsync("all_products");
        
        return createdProduct;
    }

    public async Task InvalidateProductCacheAsync(int productId)
    {
        await _cache.RemoveAsync($"product_{productId}");
    }
} 