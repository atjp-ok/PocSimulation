using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.TankModels;

namespace BFF.NewBFF.Slices.Tank.StartTank;

[ApiController]
[Route("api/[controller]")]
public class StartTankController : ControllerBase
{
    private readonly ILogger<StartTankController> _logger;
    private readonly StartTankHandler _startTankHandler;

    public StartTankController(ILogger<StartTankController> logger, StartTankHandler startTankHandler)
    {
        _logger = logger;
        _startTankHandler = startTankHandler;
    }

    /// <summary>
    /// Starts a tanking session
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("StartTank")]
    public async Task<IActionResult> StartTank([FromBody] StartTankRequest request)
    {
        _logger.LogInformation("StartTank called in BFF.");
        var result = await _startTankHandler.HandleStartTankAsync(request);
        _logger.LogInformation("StartTank completed in BFF.");
        return Ok(result);
    }
}