using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    /*
    private static readonly HttpClient _httpClient = new(); DNS resolution
    
    private readonly HttpClient _httpClient = new(); socket exhaustion 
    Opening a brand‑new HttpClient is like calling a new taxi for every single grocery item you buy instead of 
    keeping one parked for the whole trip—lots of extra traffic jams and wasted parking spaces.
    
    https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines
    */

    #region SecretSauce
    static ResiliencePipeline<HttpResponseMessage> retryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
        .AddRetry(new HttpRetryStrategyOptions
        {
            BackoffType = DelayBackoffType.Exponential,
            MaxRetryAttempts = 3
        })
        .Build();
    
    static SocketsHttpHandler socketHandler = new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(1)
    };
    
    static ResilienceHandler resilienceHandler = new ResilienceHandler(retryPipeline)
    {
        InnerHandler = socketHandler,
    };

    static readonly HttpClient _httpClient = new HttpClient(resilienceHandler);
    

    #endregion
    
    private readonly IHttpClientFactory _factory;

    public PeopleController(IHttpClientFactory factory)
        => _factory = factory;

    // GET: /api/people/john
    [HttpGet("{name}")]
    public async Task<IActionResult> GetAgePrediction(string name)
    {
        var client = _factory.CreateClient("agify");
        var response = await client.GetFromJsonAsync<AgifyResponse>($"?name={name}");

        if (response is null)
            return NotFound($"No data returned for '{name}'.");

        return Ok(response);
    }
}

public record AgifyResponse(string Name, int? Age, int Count);
