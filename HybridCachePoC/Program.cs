using HybridCachePoC.Data;
using HybridCachePoC.HealthChecks;
using HybridCachePoC.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "HybridCache PoC API", Version = "v1" });
});

// Configure Entity Framework with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgreSQL") 
        ?? "Host=localhost;Database=hybridcache;Username=hybridcache;Password=hybridcache123";
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    });
});

// Configure Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
    return ConnectionMultiplexer.Connect(connectionString);
});

// Configure Memory Cache
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = builder.Configuration.GetValue<int>("Cache:MemoryCache:SizeLimit", 1000);
    options.ExpirationScanFrequency = TimeSpan.Parse(
        builder.Configuration.GetValue<string>("Cache:MemoryCache:ExpirationScanFrequency") ?? "00:05:00");
});

// Configure Distributed Cache (Redis)
builder.Services.AddStackExchangeRedisCache(options =>
{
    var configuration = builder.Configuration;
    options.Configuration = configuration.GetConnectionString("Redis") ?? "localhost:6379";
    options.InstanceName = configuration.GetValue<string>("Redis:InstanceName") ?? "HybridCachePoC:";
});

// Register services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddScoped<DataSeeder>();

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<RedisHealthCheck>("redis", tags: new[] { "redis", "cache" })
    .AddDbContextCheck<AppDbContext>("database", tags: new[] { "database", "postgresql" });

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMetricServer(); // Exposes /metrics endpoint
app.UseHttpMetrics();  // Collects default HTTP metrics
app.UseAuthorization();

app.MapControllers();

// Health check endpoints
app.MapHealthChecks("/health");

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("database") || check.Tags.Contains("redis")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();
