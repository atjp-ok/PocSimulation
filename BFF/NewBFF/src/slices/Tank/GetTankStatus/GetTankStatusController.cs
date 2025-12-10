using Microsoft.AspNetCore.Mvc;

namespace BFF.NewBFF.Slices.Tank.GetTankStatus;

[ApiController]
[Route("api/[controller]")]
public class GetTankStatusController : ControllerBase
{
    private readonly ILogger<GetTankStatusController> _logger;
    private readonly GetTankStatusHandler _getTankStatusHandler;

    public GetTankStatusController(ILogger<GetTankStatusController> logger, GetTankStatusHandler getTankStatusHandler)
    {
        _logger = logger;
        _getTankStatusHandler = getTankStatusHandler;
    }

    [HttpGet("GetTankStatus/{tankId}")]
    public async Task<IActionResult> GetTankStatus(int tankId)
    {
        _logger.LogInformation("GetTankStatus called in BFF.");
        var result = await _getTankStatusHandler.HandleGetTankStatusAsync(tankId);
        _logger.LogInformation("GetTankStatus completed in BFF.");
        return Ok(result);
    }

}