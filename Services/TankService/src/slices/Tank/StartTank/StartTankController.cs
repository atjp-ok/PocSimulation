using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.TankModels;

namespace TankService.slices.Tank.StartTank;

[ApiController]
[Route("api/[controller]")]
public class StartTankController : ControllerBase
{
    private readonly ILogger<StartTankController> _logger;
    private readonly StartTankHandler _tankHandler;

    public StartTankController(ILogger<StartTankController> logger, StartTankHandler tankHandler)
    {
        _logger = logger;
        _tankHandler = tankHandler;
    }

    [HttpPost("StartTank")]
    public async Task<IActionResult> StartTank([FromBody] StartTankRequest request)
    {
        _logger.LogInformation($"StartTank called. StationId: {request.StationId}, Fueltype: {request.Fueltype}, UserId: {request.UserId}.");
        var result = await _tankHandler.HandleStartTank(request);
        _logger.LogInformation($"Tank started Successfully. TankId: {result.TankId}, Fuelttype: {request.Fueltype} for user {request.UserId}.");
        return Ok(result);
    }
}