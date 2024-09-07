using Polly;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

//old way
app.MapGet("/weatherforecastold", async () =>
{
    var policy = new ClientRetryPolicy();

    var client = new HttpClient();

    var response = await policy.JustHttpRetry.ExecuteAsync(() =>
    {
        return client.GetAsync("https://localhost:7133/notify/sms/test");
    });
})
.WithName("GetWeatherForecastOld");

//new way
app.MapGet("/weatherforecast", async (CancellationToken token) =>
{
    ResiliencePipeline resiliencePipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            OnRetry = static args =>
            {
                Console.WriteLine("OnRetry, Attempt: {0}", args.AttemptNumber);

                // Event handlers can be asynchronous; here, we return an empty ValueTask.
                return default;
            },
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Linear
            //BackoffType = DelayBackoffType.Constant
            //BackoffType = DelayBackoffType.Exponential
        })
        .Build();

    var result = await resiliencePipeline.ExecuteAsync(static async token =>
    {
        var client = new HttpClient();
        var response = await client.GetAsync("https://localhost:7133/notify/sms/test", token);

        return response;
    }, token);

    return result;
});



app.Run();




/// <summary>
/// Old way with Polly
/// </summary>
public class ClientRetryPolicy
{
    public AsyncRetryPolicy<HttpResponseMessage> JustHttpRetry { get; set; }
    public AsyncRetryPolicy<HttpResponseMessage> HttpRetryWithWaiting { get; set; }
    public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; set; }

    public ClientRetryPolicy()
    {
        JustHttpRetry = Policy.HandleResult<HttpResponseMessage>(
            response => !response.IsSuccessStatusCode)
            .RetryAsync(3);

        HttpRetryWithWaiting = Policy.HandleResult<HttpResponseMessage>(
            response => !response.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(5));

        ExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
            response => !response.IsSuccessStatusCode)
            .WaitAndRetryAsync(3,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }
}