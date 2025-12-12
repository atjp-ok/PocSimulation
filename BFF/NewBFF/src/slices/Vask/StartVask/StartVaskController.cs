using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.VaskModels;

namespace BFF.NewBFF.Slices.Vask.StartVask;

[ApiController]
[Route("api/[controller]")]
public class StartVaskController : ControllerBase
{
    private readonly ILogger<StartVaskController> _logger;
    private readonly StartVaskHandler _startVaskHandler;

    public StartVaskController(ILogger<StartVaskController> logger, StartVaskHandler startVaskHandler)
    {
        _logger = logger;
        _startVaskHandler = startVaskHandler;
    }

    /// <summary>
    /// Starts a vask session
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("StartVask")]
    public async Task<IActionResult> StartVask([FromBody] VaskRequest request)
    {
        _logger.LogInformation("StartVask called in BFF.");
        var result = await _startVaskHandler.HandleStartVaskAsync(request);
        _logger.LogInformation("StartVask completed in BFF.");
        return Ok(result);
    }
}