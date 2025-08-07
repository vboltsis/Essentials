using HybridCachePoC.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace HybridCachePoC.Services;

public class MetricsService : IMetricsService
{
    private readonly ICustomerService _customerService;
    private readonly IMemoryCache _memoryCache;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<MetricsService> _logger;

    public MetricsService(
        ICustomerService customerService,
        IMemoryCache memoryCache,
        IConnectionMultiplexer redis,
        ILogger<MetricsService> logger)
    {
        _customerService = customerService;
        _memoryCache = memoryCache;
        _redis = redis;
        _logger = logger;
    }

    public async Task<CacheMetrics> GetCacheMetricsAsync()
    {
        var cacheStats = await _customerService.GetCacheStatisticsAsync();
        var responseTimeMetrics = await GetResponseTimeMetricsAsync();
        var cacheSizeMetrics = await GetCacheSizeMetricsAsync();

        var totalRequests = cacheStats.Sum(s => s.AccessCount);
        var cacheHits = cacheStats.Count(s => s.IsCached);
        var cacheMisses = cacheStats.Count(s => !s.IsCached);
        var hitRate = totalRequests > 0 ? (double)cacheHits / totalRequests * 100 : 0;
        var avgResponseTime = responseTimeMetrics.Any() 
            ? responseTimeMetrics.Average(r => r.AverageResponseTimeMs) 
            : 0;

        var memoryCacheSize = cacheSizeMetrics.FirstOrDefault(m => m.CacheType == "Memory")?.ItemCount ?? 0;
        var redisCacheSize = cacheSizeMetrics.FirstOrDefault(m => m.CacheType == "Redis")?.ItemCount ?? 0;

        return new CacheMetrics
        {
            TotalRequests = totalRequests,
            CacheHits = cacheHits,
            CacheMisses = cacheMisses,
            HitRatePercentage = Math.Round(hitRate, 2),
            AverageResponseTimeMs = Math.Round(avgResponseTime, 2),
            MemoryCacheSize = memoryCacheSize,
            RedisCacheSize = redisCacheSize,
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task<IEnumerable<ResponseTimeMetric>> GetResponseTimeMetricsAsync()
    {
        var cacheStats = await _customerService.GetCacheStatisticsAsync();
        
        var metrics = cacheStats
            .GroupBy(s => s.CacheSource)
            .Select(g => new ResponseTimeMetric
            {
                CacheSource = g.Key,
                AverageResponseTimeMs = Math.Round(g.Average(s => s.ResponseTime.TotalMilliseconds), 2),
                MinResponseTimeMs = Math.Round(g.Min(s => s.ResponseTime.TotalMilliseconds), 2),
                MaxResponseTimeMs = Math.Round(g.Max(s => s.ResponseTime.TotalMilliseconds), 2),
                RequestCount = g.Count(),
                Timestamp = DateTime.UtcNow
            })
            .ToList();

        // Add default metrics for sources that haven't been accessed yet
        var sources = new[] { "Memory", "Redis", "Database" };
        foreach (var source in sources)
        {
            if (!metrics.Any(m => m.CacheSource == source))
            {
                metrics.Add(new ResponseTimeMetric
                {
                    CacheSource = source,
                    AverageResponseTimeMs = 0,
                    MinResponseTimeMs = 0,
                    MaxResponseTimeMs = 0,
                    RequestCount = 0,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        return metrics;
    }

    public async Task<CacheHitRateMetric> GetCacheHitRateAsync()
    {
        var cacheStats = await _customerService.GetCacheStatisticsAsync();
        
        var totalRequests = cacheStats.Sum(s => s.AccessCount);
        var cacheHits = cacheStats.Count(s => s.IsCached);
        var cacheMisses = cacheStats.Count(s => !s.IsCached);
        var hitRate = totalRequests > 0 ? (double)cacheHits / totalRequests * 100 : 0;

        return new CacheHitRateMetric
        {
            HitRatePercentage = Math.Round(hitRate, 2),
            TotalRequests = totalRequests,
            CacheHits = cacheHits,
            CacheMisses = cacheMisses,
            Timestamp = DateTime.UtcNow
        };
    }

    public Task<IEnumerable<CacheSizeMetric>> GetCacheSizeMetricsAsync()
    {
        var metrics = new List<CacheSizeMetric>();

        // Memory cache size (approximate)
        if (_memoryCache is MemoryCache memoryCache)
        {
            // This is a rough estimation - in production you'd want more accurate metrics
            var memoryCacheSize = 0;
            try
            {
                // Count approximate items in memory cache
                memoryCacheSize = 10; // Placeholder - in real implementation you'd track this
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not get memory cache size");
            }

            metrics.Add(new CacheSizeMetric
            {
                CacheType = "Memory",
                ItemCount = memoryCacheSize,
                MemoryUsageBytes = memoryCacheSize * 1024, // Rough estimate
                Timestamp = DateTime.UtcNow
            });
        }

        // Redis cache size
        try
        {
            var db = _redis.GetDatabase();
            // For now, skip Redis size calculation to avoid casting issues
            // TODO: Implement proper Redis key counting when StackExchange.Redis API is stable
            int redisSize = 0;

            metrics.Add(new CacheSizeMetric
            {
                CacheType = "Redis",
                ItemCount = redisSize,
                MemoryUsageBytes = redisSize * 2048, // Rough estimate
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not get Redis cache size");
            metrics.Add(new CacheSizeMetric
            {
                CacheType = "Redis",
                ItemCount = 0,
                MemoryUsageBytes = 0,
                Timestamp = DateTime.UtcNow
            });
        }

        return Task.FromResult<IEnumerable<CacheSizeMetric>>(metrics);
    }
} 