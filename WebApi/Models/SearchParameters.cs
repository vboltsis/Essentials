using Microsoft.AspNetCore.Mvc;

namespace WeatherExample;

public readonly struct SearchParameters
{
    [FromQuery(Name = "q")]
    public string Query { get; init; }

    [FromQuery(Name = "page")]
    public int Page { get; init; }

    [FromHeader(Name = "X-Custom-Header")]
    public string CustomHeader { get; init; }

    [FromServices]
    public IHelloService Service { get; init; }
}

