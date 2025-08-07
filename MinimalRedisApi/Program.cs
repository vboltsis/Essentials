using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.OpenApi.Models;
using MinimalRedisApi.Models;
using MinimalRedisApi.Repositories;
using MinimalRedisApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HybridCache API",
        Version = "v1",
        Description = "A .NET 9 Minimal API demonstrating HybridCache with Redis integration"
    });
});

var redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION");

string connectionString;
if (!string.IsNullOrEmpty(redisConnection))
{
    connectionString = redisConnection;
}
else
{
    connectionString = "localhost:6379";
}

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = connectionString;
});

builder.Services.AddHybridCache(x =>
{
    x.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(5),
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };
});

builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TraditionalCacheService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HybridCache API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

// HybridCache endpoints
app.MapGet("/hybrid/products/{id}", async (int id, ProductService productService) =>
{
    try
    {
        var product = await productService.GetProductAsync(id);
        return product != null ? Results.Ok(product) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving product: {ex.Message}");
    }
})
.WithName("GetProduct")
.WithOpenApi()
.WithTags("HybridCache");

app.MapGet("/hybrid/products", async (ProductService productService) =>
{
    try
    {
        var products = await productService.GetAllProductsAsync();
        return Results.Ok(products);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving products: {ex.Message}");
    }
})
.WithName("GetAllProducts")
.WithOpenApi()
.WithTags("HybridCache");

app.MapPost("/hybrid/products", async (Product product, ProductService productService) =>
{
    try
    {
        var createdProduct = await productService.CreateProductAsync(product);
        return Results.Created($"/hybrid/products/{createdProduct.Id}", createdProduct);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error creating product: {ex.Message}");
    }
})
.WithName("CreateProduct")
.WithOpenApi()
.WithTags("HybridCache");

app.MapDelete("/hybrid/products/{id}/cache", async (int id, ProductService productService) =>
{
    await productService.InvalidateProductCacheAsync(id);
    return Results.Ok($"Cache invalidated for product {id}");
})
.WithName("InvalidateProductCache")
.WithOpenApi()
.WithTags("HybridCache");

// Traditional cache endpoints for comparison
app.MapGet("/traditional/products/{id}", async (int id, TraditionalCacheService cacheService) =>
{
    try
    {
        var product = await cacheService.GetProductAsync(id);
        return product != null ? Results.Ok(product) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving product: {ex.Message}");
    }
})
.WithName("GetProductTraditional")
.WithOpenApi()
.WithTags("Traditional Cache");

app.MapGet("/traditional/products", async (TraditionalCacheService cacheService) =>
{
    try
    {
        var products = await cacheService.GetAllProductsAsync();
        return Results.Ok(products);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving products: {ex.Message}");
    }
})
.WithName("GetAllProductsTraditional")
.WithOpenApi()
.WithTags("Traditional Cache");

// Health check endpoint
app.MapGet("/health", () => Results.Ok("HybridCache API is running!"))
.WithName("Health")
.WithOpenApi()
.WithTags("Health");

app.Run();
