using HybridCachePoC.Models;

namespace HybridCachePoC.Services;

public interface IMetricsService
{
    Task<CacheMetrics> GetCacheMetricsAsync();
    Task<IEnumerable<ResponseTimeMetric>> GetResponseTimeMetricsAsync();
    Task<CacheHitRateMetric> GetCacheHitRateAsync();
    Task<IEnumerable<CacheSizeMetric>> GetCacheSizeMetricsAsync();
}

public class CacheMetrics
{
    public int TotalRequests { get; set; }
    public int CacheHits { get; set; }
    public int CacheMisses { get; set; }
    public double HitRatePercentage { get; set; }
    public double AverageResponseTimeMs { get; set; }
    public int MemoryCacheSize { get; set; }
    public int RedisCacheSize { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ResponseTimeMetric
{
    public string CacheSource { get; set; } = string.Empty;
    public double AverageResponseTimeMs { get; set; }
    public double MinResponseTimeMs { get; set; }
    public double MaxResponseTimeMs { get; set; }
    public int RequestCount { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class CacheHitRateMetric
{
    public double HitRatePercentage { get; set; }
    public int TotalRequests { get; set; }
    public int CacheHits { get; set; }
    public int CacheMisses { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class CacheSizeMetric
{
    public string CacheType { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public long MemoryUsageBytes { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
} 