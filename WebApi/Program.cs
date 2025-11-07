using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WeatherExample;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(/*x => x.Filters.Add<ApiKeyAuthFilter>()*/);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "The API Key to access the API",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };

    c.AddSecurityRequirement(requirement);
});

builder.Services.AddHttpClient("agify", client =>
{
    client.BaseAddress = new Uri("https://api.agify.io/");
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddSingleton<IRuntimeInformationService, RuntimeInformationService>();
builder.Services.AddSingleton<IMemoryMetricsService, MemoryMetricsService>();
builder.Services.AddScoped<ApiKeyAuthFilter>();
//EXAMPLE SERVICES
builder.Services.AddTransient<ITransientCounterService, CounterService>(); // one instance per request per service
builder.Services.AddScoped<IScopedCounterService, CounterService>(); // one instance per request
builder.Services.AddSingleton<ISingletonCounterService, CounterService>(); //one instance for the whole application
builder.Services.AddTransient<IAnotherService, AnotherService>();

builder.Services.AddKeyedSingleton<INotificationService, SmsNotificationService>("sms");
builder.Services.AddKeyedSingleton<INotificationService, EmailNotificationService>("email");
builder.Services.AddSingleton<IHelloService, HelloService>();

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

var group = app.MapGroup("Weather").AddEndpointFilter<ApiKeyEndpointFilter>();

group.MapGet("weathermini", () =>
{
    return Results.Ok();
}).AddEndpointFilter<ApiKeyEndpointFilter>();

app.MapGet("/parameters", ([AsParameters] SearchParameters parameters) =>
{
    var results = parameters.Service.SayHello(parameters.Query);
    return Results.Ok(new
    {
        Query = parameters.Query,
        Page = parameters.Page,
        CustomHeader = parameters.CustomHeader,
        Results = results
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapFallback(async (context) =>
{
    var unmatchedPath = context.Request.Path.Value;
    Console.WriteLine(unmatchedPath);
    
    await context.Response.WriteAsync("Path Not Found");
});

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
