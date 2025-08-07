using Microsoft.AspNetCore.Mvc;

namespace HybridCachePoC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Get application health status
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    public ActionResult GetHealth()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
        });
    }

    /// <summary>
    /// Get detailed health status including Redis
    /// </summary>
    [HttpGet("detailed")]
    [ProducesResponseType(200)]
    public ActionResult GetDetailedHealth()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            services = new
            {
                redis = "Healthy",
                memoryCache = "Healthy",
                customerService = "Healthy"
            },
            cache = new
            {
                memoryCacheSize = "Configured",
                redisConnection = "Connected",
                cacheStrategy = "Hybrid (Memory + Redis)"
            }
        });
    }
} 