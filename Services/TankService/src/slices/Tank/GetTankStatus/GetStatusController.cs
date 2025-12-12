using Microsoft.AspNetCore.Mvc;

namespace TankService.slices.Tank.GetTankStatus;

[ApiController]
[Route("api/[controller]")]
public class GetTankStatusController : ControllerBase
{
    private readonly ILogger<GetTankStatusController> _logger;
    private readonly GetTankStatusHandler _statusHandler;

    public GetTankStatusController(ILogger<GetTankStatusController> logger, GetTankStatusHandler statusHandler)
    {
        _logger = logger;
        _statusHandler = statusHandler;
    }

    /// <summary>
    /// Gets the current status of a tanking session.
    /// </summary>
    [HttpGet("GetTankStatus/{tankId}")]
    public async Task<IActionResult> GetTankStatus(int tankId) 
    {
        _logger.LogInformation($"GetTankStatus called. Polling current status for TankId: {tankId}.");
        var result = await _statusHandler.HandleTankStatus(tankId);
        if (result == null)
        {
            _logger.LogInformation($"Tank not found. TankId: {tankId}.");
            return NotFound();
        }
        _logger.LogInformation($"Tank status retrieved. TankId: {tankId}, Status: {result.CurrentStatus}.");
        return Ok(result);
    }

}