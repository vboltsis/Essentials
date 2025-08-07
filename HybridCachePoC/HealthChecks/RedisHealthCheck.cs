using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace HybridCachePoC.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisHealthCheck> _logger;

    public RedisHealthCheck(IConnectionMultiplexer redis, ILogger<RedisHealthCheck> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_redis.IsConnected)
            {
                _logger.LogWarning("Redis is not connected");
                return HealthCheckResult.Unhealthy("Redis is not connected");
            }

            var db = _redis.GetDatabase();
            var pong = await db.PingAsync();
            
            _logger.LogInformation("Redis health check successful. Ping: {Ping}ms", pong.TotalMilliseconds);
            
            return pong.TotalMilliseconds < 100 
                ? HealthCheckResult.Healthy($"Redis is healthy. Ping: {pong.TotalMilliseconds}ms")
                : HealthCheckResult.Degraded($"Redis is slow. Ping: {pong.TotalMilliseconds}ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis health check failed");
            return HealthCheckResult.Unhealthy("Redis health check failed", ex);
        }
    }
} 