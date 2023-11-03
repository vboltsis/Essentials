using Microsoft.EntityFrameworkCore;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRuntimeInformationService, RuntimeInformationService>();
builder.Services.AddSingleton<IMemoryMetricsService, MemoryMetricsService>();

//EXAMPLE SERVICES
builder.Services.AddTransient<ITransientCounterService, CounterService>(); // one instance per request per service
builder.Services.AddScoped<IScopedCounterService, CounterService>(); // one instance per request
builder.Services.AddSingleton<ISingletonCounterService, CounterService>(); //one instance for the whole application
builder.Services.AddTransient<IAnotherService, AnotherService>();


builder.Services.AddDbContextPool<WeatherContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WeatherDB"));
    options.EnableThreadSafetyChecks(false);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
