using Microsoft.EntityFrameworkCore;
using WeatherExample;
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

builder.Services.AddKeyedSingleton<INotificationService, SmsNotificationService>("sms");
builder.Services.AddKeyedSingleton<INotificationService, EmailNotificationService>("email");

builder.Services.AddDbContextPool<WeatherContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WeatherDB"));
    options.EnableThreadSafetyChecks(false);
});

var app = builder.Build();

app.MapGet("/notify/sms/{message}", ([FromKeyedServices("sms")] INotificationService smsService, string message) =>
{
    return smsService.Notify(message);
});

app.MapGet("/notify/email/{message}", ([FromKeyedServices("email")] INotificationService emailService, string message) =>
{
    return emailService.Notify(message);
});

app.MapGet("/fetchdata", async (CancellationToken cancellationToken) =>
{
    try
    {
        // Simulating a long-running task
        var data = await FetchDataAsync(cancellationToken);
        return Results.Ok(data);
    }
    catch (OperationCanceledException ex)
    {
        return Results.Problem($"The request was cancelled. {ex}");
    }

    static async Task<string> FetchDataAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 10; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(1000, cancellationToken); // Simulate work
        }
        return "Fetched data successfully!";
    }
});

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
