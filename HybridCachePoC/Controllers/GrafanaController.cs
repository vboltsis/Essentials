using HybridCachePoC.Services;
using Microsoft.AspNetCore.Mvc;

namespace HybridCachePoC.Controllers;

[ApiController]
[Route("api/grafana")]
public class GrafanaController : ControllerBase
{
    private readonly IMetricsService _metricsService;
    private readonly ILogger<GrafanaController> _logger;

    public GrafanaController(IMetricsService metricsService, ILogger<GrafanaController> logger)
    {
        _metricsService = metricsService;
        _logger = logger;
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] GrafanaQueryRequest request)
    {
        try
        {
            var metrics = await _metricsService.GetCacheMetricsAsync();
            var responseTimeMetrics = await _metricsService.GetResponseTimeMetricsAsync();
            var cacheSizeMetrics = await _metricsService.GetCacheSizeMetricsAsync();

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var response = new List<GrafanaQueryResponse>();

            foreach (var target in request.Targets)
            {
                var targetResponse = new GrafanaQueryResponse
                {
                    Target = target.Target,
                    Datapoints = new List<object[]>()
                };

                switch (target.Target?.ToLower())
                {
                    case "cache_hit_rate":
                        targetResponse.Datapoints.Add(new object[] { metrics.HitRatePercentage, timestamp });
                        break;

                    case "average_response_time":
                        targetResponse.Datapoints.Add(new object[] { metrics.AverageResponseTimeMs, timestamp });
                        break;

                    case "total_requests":
                        targetResponse.Datapoints.Add(new object[] { metrics.TotalRequests, timestamp });
                        break;

                    case "cache_hits":
                        targetResponse.Datapoints.Add(new object[] { metrics.CacheHits, timestamp });
                        break;

                    case "cache_misses":
                        targetResponse.Datapoints.Add(new object[] { metrics.CacheMisses, timestamp });
                        break;

                    case "memory_cache_size":
                        var memorySize = cacheSizeMetrics.FirstOrDefault(m => m.CacheType == "Memory");
                        targetResponse.Datapoints.Add(new object[] { memorySize?.MemoryUsageBytes ?? 0, timestamp });
                        break;

                    case "redis_cache_size":
                        var redisSize = cacheSizeMetrics.FirstOrDefault(m => m.CacheType == "Redis");
                        targetResponse.Datapoints.Add(new object[] { redisSize?.MemoryUsageBytes ?? 0, timestamp });
                        break;

                    case "response_time_by_source":
                        foreach (var responseMetric in responseTimeMetrics)
                        {
                            targetResponse.Datapoints.Add(new object[] { responseMetric.AverageResponseTimeMs, timestamp });
                        }
                        break;

                    case "cache_metrics_summary":
                        targetResponse.Datapoints.Add(new object[] { 
                            $"Hit Rate: {metrics.HitRatePercentage:F1}% | Requests: {metrics.TotalRequests} | Avg Response: {metrics.AverageResponseTimeMs:F1}ms", 
                            timestamp 
                        });
                        break;
                }

                response.Add(targetResponse);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Grafana query");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    [HttpGet("search")]
    public IActionResult Search()
    {
        var targets = new[]
        {
            "cache_hit_rate",
            "average_response_time",
            "total_requests",
            "cache_hits",
            "cache_misses",
            "memory_cache_size",
            "redis_cache_size",
            "response_time_by_source",
            "cache_metrics_summary"
        };

        return Ok(targets);
    }

    [HttpGet("annotations")]
    public IActionResult Annotations()
    {
        return Ok(new List<object>());
    }
}

public class GrafanaQueryRequest
{
    public string? RangeRaw { get; set; }
    public GrafanaTimeRange? Range { get; set; }
    public List<GrafanaTarget> Targets { get; set; } = new();
    public string? Format { get; set; }
    public long? MaxDataPoints { get; set; }
}

public class GrafanaTimeRange
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? Raw { get; set; }
}

public class GrafanaTarget
{
    public string? Target { get; set; }
    public string? RefId { get; set; }
    public string? Type { get; set; }
}

public class GrafanaQueryResponse
{
    public string? Target { get; set; }
    public List<object[]> Datapoints { get; set; } = new();
} 