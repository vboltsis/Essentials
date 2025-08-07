using System.Diagnostics;
using HybridCachePoC.Data;
using HybridCachePoC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HybridCachePoC.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CustomerService> _logger;
    private readonly Dictionary<Guid, CustomerCacheInfo> _cacheStats = new();

    private const string CustomerCacheKeyPrefix = "customer:";
    private const string AllCustomersCacheKey = "customers:all";
    private const int MemoryCacheExpirationMinutes = 30;
    private const int RedisCacheExpirationMinutes = 60;

    public CustomerService(
        AppDbContext context,
        IMemoryCache memoryCache,
        IDistributedCache distributedCache,
        ILogger<CustomerService> logger)
    {
        _context = context;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Customer?> GetCustomerAsync(Guid id)
    {
        var stopwatch = Stopwatch.StartNew();
        var cacheKey = $"{CustomerCacheKeyPrefix}{id}";
        var cacheInfo = new CustomerCacheInfo { CustomerId = id };

        try
        {
            // Try memory cache first (fastest)
            if (_memoryCache.TryGetValue(cacheKey, out Customer? customer))
            {
                stopwatch.Stop();
                cacheInfo.IsCached = true;
                cacheInfo.CacheSource = "Memory";
                cacheInfo.ResponseTime = stopwatch.Elapsed;
                UpdateCacheStats(id, cacheInfo);
                
                _logger.LogInformation("Customer {CustomerId} retrieved from memory cache in {ResponseTime}ms", 
                    id, stopwatch.ElapsedMilliseconds);
                
                return customer;
            }

            // Try Redis cache
            var redisValue = await _distributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(redisValue))
            {
                customer = JsonSerializer.Deserialize<Customer>(redisValue);
                if (customer != null)
                {
                    // Store in memory cache for faster subsequent access
                    var memoryCacheOptionsForRedis = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(MemoryCacheExpirationMinutes),
                        SlidingExpiration = TimeSpan.FromMinutes(10),
                        Size = 1
                    };
                    _memoryCache.Set(cacheKey, customer, memoryCacheOptionsForRedis);

                    stopwatch.Stop();
                    cacheInfo.IsCached = true;
                    cacheInfo.CacheSource = "Redis";
                    cacheInfo.ResponseTime = stopwatch.Elapsed;
                    UpdateCacheStats(id, cacheInfo);
                    
                    _logger.LogInformation("Customer {CustomerId} retrieved from Redis cache in {ResponseTime}ms", 
                        id, stopwatch.ElapsedMilliseconds);
                    
                    return customer;
                }
            }

            // Get from database
            customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                // Update access tracking
                customer.LastAccessed = DateTime.UtcNow;
                customer.AccessCount++;
                await _context.SaveChangesAsync();

                // Cache in both memory and Redis
                var memoryCacheOptionsForDb = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(MemoryCacheExpirationMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    Size = 1
                };
                _memoryCache.Set(cacheKey, customer, memoryCacheOptionsForDb);

                var redisOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(RedisCacheExpirationMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(15)
                };
                await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(customer), redisOptions);

                stopwatch.Stop();
                cacheInfo.IsCached = false;
                cacheInfo.CacheSource = "Database";
                cacheInfo.ResponseTime = stopwatch.Elapsed;
                UpdateCacheStats(id, cacheInfo);
                
                _logger.LogInformation("Customer {CustomerId} retrieved from database and cached in {ResponseTime}ms", 
                    id, stopwatch.ElapsedMilliseconds);
            }

            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer {CustomerId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var cacheKey = AllCustomersCacheKey;

        try
        {
            // Try memory cache first
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Customer>? customers))
            {
                stopwatch.Stop();
                _logger.LogInformation("All customers retrieved from memory cache in {ResponseTime}ms", 
                    stopwatch.ElapsedMilliseconds);
                return customers ?? Enumerable.Empty<Customer>();
            }

            // Try Redis cache
            var redisValue = await _distributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(redisValue))
            {
                customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(redisValue);
                if (customers != null)
                {
                    // Store in memory cache
                    var memoryCacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(MemoryCacheExpirationMinutes),
                        SlidingExpiration = TimeSpan.FromMinutes(10),
                        Size = 1
                    };
                    _memoryCache.Set(cacheKey, customers, memoryCacheOptions);

                    stopwatch.Stop();
                    _logger.LogInformation("All customers retrieved from Redis cache in {ResponseTime}ms", 
                        stopwatch.ElapsedMilliseconds);
                    return customers;
                }
            }

            // Get from database
            customers = await _context.Customers
                .Where(c => c.IsActive)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
            
            // Cache in both memory and Redis
            var memoryCacheOptionsForAll = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(MemoryCacheExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(10),
                Size = 1
            };
            _memoryCache.Set(cacheKey, customers, memoryCacheOptionsForAll);

            var redisOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(RedisCacheExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(customers), redisOptions);

            stopwatch.Stop();
            _logger.LogInformation("All customers retrieved from database and cached in {ResponseTime}ms", 
                stopwatch.ElapsedMilliseconds);
            
            return customers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all customers");
            throw;
        }
    }

    public async Task<Customer> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Company = request.Company,
            Address = request.Address,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        
        // Invalidate cache
        await InvalidateCustomerCacheAsync(customer.Id);
        await InvalidateAllCustomersCacheAsync();
        
        _logger.LogInformation("Customer {CustomerId} created successfully", customer.Id);
        return customer;
    }

    public async Task<Customer?> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return null;

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.FirstName))
            customer.FirstName = request.FirstName;
        if (!string.IsNullOrEmpty(request.LastName))
            customer.LastName = request.LastName;
        if (!string.IsNullOrEmpty(request.Email))
            customer.Email = request.Email;
        if (request.PhoneNumber != null)
            customer.PhoneNumber = request.PhoneNumber;
        if (request.Company != null)
            customer.Company = request.Company;
        if (request.Address != null)
            customer.Address = request.Address;
        if (request.City != null)
            customer.City = request.City;
        if (request.State != null)
            customer.State = request.State;
        if (request.PostalCode != null)
            customer.PostalCode = request.PostalCode;
        if (request.Country != null)
            customer.Country = request.Country;
        if (request.Notes != null)
            customer.Notes = request.Notes;
        if (request.IsActive.HasValue)
            customer.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync();
        
        // Invalidate cache
        await InvalidateCustomerCacheAsync(id);
        await InvalidateAllCustomersCacheAsync();
        
        _logger.LogInformation("Customer {CustomerId} updated successfully", id);
        return customer;
    }

    public async Task<bool> DeleteCustomerAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        
        // Invalidate cache
        await InvalidateCustomerCacheAsync(id);
        await InvalidateAllCustomersCacheAsync();
        
        _logger.LogInformation("Customer {CustomerId} deleted successfully", id);
        return true;
    }

    public Task<CustomerCacheInfo> GetCustomerCacheInfoAsync(Guid id)
    {
        if (_cacheStats.TryGetValue(id, out var cacheInfo))
        {
            return Task.FromResult(cacheInfo);
        }

        return Task.FromResult(new CustomerCacheInfo
        {
            CustomerId = id,
            LastAccessed = DateTime.UtcNow,
            AccessCount = 0,
            IsCached = false,
            CacheSource = "Not accessed",
            ResponseTime = TimeSpan.Zero
        });
    }

    public Task<IEnumerable<CustomerCacheInfo>> GetCacheStatisticsAsync()
    {
        return Task.FromResult(_cacheStats.Values.AsEnumerable());
    }

    public Task ClearCacheAsync()
    {
        // Clear memory cache
        if (_memoryCache is MemoryCache memoryCache)
        {
            memoryCache.Compact(1.0);
        }

        // Clear Redis cache (this would require a more sophisticated approach in production)
        _logger.LogInformation("Cache cleared successfully");
        return Task.CompletedTask;
    }

    public async Task WarmCacheAsync()
    {
        _logger.LogInformation("Starting cache warming...");
        
        var customers = await _context.Customers
            .Where(c => c.IsActive)
            .ToListAsync();
        
        var tasks = customers.Select(async customer =>
        {
            var cacheKey = $"{CustomerCacheKeyPrefix}{customer.Id}";
            
            // Cache in memory
            var memoryCacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(MemoryCacheExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(10),
                Size = 1
            };
            _memoryCache.Set(cacheKey, customer, memoryCacheOptions);

            // Cache in Redis
            var redisOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(RedisCacheExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(customer), redisOptions);
        });

        await Task.WhenAll(tasks);
        
        // Cache all customers list
        var allCustomersCacheKey = AllCustomersCacheKey;
        var memoryCacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(MemoryCacheExpirationMinutes),
            SlidingExpiration = TimeSpan.FromMinutes(10),
            Size = 1
        };
        _memoryCache.Set(allCustomersCacheKey, customers, memoryCacheOptions);

        var redisOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(RedisCacheExpirationMinutes),
            SlidingExpiration = TimeSpan.FromMinutes(15)
        };
        await _distributedCache.SetStringAsync(allCustomersCacheKey, JsonSerializer.Serialize(customers), redisOptions);

        _logger.LogInformation("Cache warming completed for {CustomerCount} customers", customers.Count);
    }

    private async Task InvalidateCustomerCacheAsync(Guid id)
    {
        var cacheKey = $"{CustomerCacheKeyPrefix}{id}";
        _memoryCache.Remove(cacheKey);
        await _distributedCache.RemoveAsync(cacheKey);
    }

    private async Task InvalidateAllCustomersCacheAsync()
    {
        _memoryCache.Remove(AllCustomersCacheKey);
        await _distributedCache.RemoveAsync(AllCustomersCacheKey);
    }

    private void UpdateCacheStats(Guid id, CustomerCacheInfo cacheInfo)
    {
        if (_cacheStats.ContainsKey(id))
        {
            var existing = _cacheStats[id];
            existing.AccessCount++;
            existing.LastAccessed = DateTime.UtcNow;
            existing.IsCached = cacheInfo.IsCached;
            existing.CacheSource = cacheInfo.CacheSource;
            existing.ResponseTime = cacheInfo.ResponseTime;
        }
        else
        {
            cacheInfo.AccessCount = 1;
            _cacheStats[id] = cacheInfo;
        }
    }
} 