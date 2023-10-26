using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CounterController : ControllerBase
{
    private readonly ITransientCounterService _transientCounter;
    private readonly IScopedCounterService _scopedCounter;
    private readonly ISingletonCounterService _singletonCounter;
    private readonly IAnotherService _anotherService;

    public CounterController(
        ITransientCounterService transientCounter,
        IScopedCounterService scopedCounter,
        ISingletonCounterService singletonCounter,
        IAnotherService anotherService)
    {
        _transientCounter = transientCounter;
        _scopedCounter = scopedCounter;
        _singletonCounter = singletonCounter;
        _anotherService = anotherService;
    }

    [HttpGet]
    public IActionResult GetCounts()
    {
        var one = _anotherService.GetScopedCount();
        var two = _transientCounter.IncreaseAndGet();
        var three = _scopedCounter.IncreaseAndGet();
        var four = _singletonCounter.IncreaseAndGet();
        var five = _singletonCounter.IncreaseAndGet();

        return Ok();
    }
}