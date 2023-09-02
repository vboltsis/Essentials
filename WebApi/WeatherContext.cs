using Microsoft.EntityFrameworkCore;

namespace WebApi;

public class WeatherContext : DbContext
{
    public WeatherContext(DbContextOptions<WeatherContext> options)
        : base(options)
    {
    }
}
