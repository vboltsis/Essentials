using HybridCachePoC.Services;
using Microsoft.AspNetCore.Mvc;

namespace HybridCachePoC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricsController : ControllerBase
{
    private readonly IMetricsService _metricsService;
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(IMetricsService metricsService, ILogger<MetricsController> logger)
    {
        _metricsService = metricsService;
        _logger = logger;
    }

    /// <summary>
    /// Get comprehensive cache metrics for Grafana dashboard
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CacheMetrics), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CacheMetrics>> GetCacheMetrics()
    {
        try
        {
            var metrics = await _metricsService.GetCacheMetricsAsync();
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cache metrics");
            return StatusCode(500, "An error occurred while retrieving cache metrics");
        }
    }

    /// <summary>
    /// Get response time metrics by cache source
    /// </summary>
    [HttpGet("response-times")]
    [ProducesResponseType(typeof(IEnumerable<ResponseTimeMetric>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<ResponseTimeMetric>>> GetResponseTimeMetrics()
    {
        try
        {
            var metrics = await _metricsService.GetResponseTimeMetricsAsync();
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving response time metrics");
            return StatusCode(500, "An error occurred while retrieving response time metrics");
        }
    }

    /// <summary>
    /// Get cache hit rate metrics
    /// </summary>
    [HttpGet("hit-rate")]
    [ProducesResponseType(typeof(CacheHitRateMetric), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<CacheHitRateMetric>> GetCacheHitRate()
    {
        try
        {
            var metrics = await _metricsService.GetCacheHitRateAsync();
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cache hit rate metrics");
            return StatusCode(500, "An error occurred while retrieving cache hit rate metrics");
        }
    }

    /// <summary>
    /// Get cache size metrics
    /// </summary>
    [HttpGet("cache-size")]
    [ProducesResponseType(typeof(IEnumerable<CacheSizeMetric>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<CacheSizeMetric>>> GetCacheSizeMetrics()
    {
        try
        {
            var metrics = await _metricsService.GetCacheSizeMetricsAsync();
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cache size metrics");
            return StatusCode(500, "An error occurred while retrieving cache size metrics");
        }
    }

    /// <summary>
    /// Get all metrics in a single response for Grafana
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetAllMetrics()
    {
        try
        {
            var cacheMetrics = await _metricsService.GetCacheMetricsAsync();
            var responseTimeMetrics = await _metricsService.GetResponseTimeMetricsAsync();
            var hitRateMetrics = await _metricsService.GetCacheHitRateAsync();
            var cacheSizeMetrics = await _metricsService.GetCacheSizeMetricsAsync();

            var allMetrics = new
            {
                cache = cacheMetrics,
                responseTimes = responseTimeMetrics,
                hitRate = hitRateMetrics,
                cacheSize = cacheSizeMetrics,
                timestamp = DateTime.UtcNow
            };

            return Ok(allMetrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all metrics");
            return StatusCode(500, "An error occurred while retrieving metrics");
        }
    }
} 