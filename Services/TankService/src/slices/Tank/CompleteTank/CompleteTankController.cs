using Microsoft.AspNetCore.Mvc;
using TankService.slices.CompleteTank;
using Shared.SharedModels.TankModels;

namespace TankService.slices.Tank.CompleteTank;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "'Internal'")]
public class CompleteTankController : ControllerBase
{
    private readonly ILogger<CompleteTankController> _logger;
    private readonly CompleteTankHandler _tankHandler;

    public CompleteTankController(ILogger<CompleteTankController> logger, CompleteTankHandler tankHandler)
    {
        _logger = logger;
        _tankHandler = tankHandler;
    }

    /// <summary>
    /// Internal Endpoint called by Hammaq to complete a tanking session
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("CompleteTank")]
    public async Task<IActionResult> CompleteTank([FromBody] TankCompleteRequest request)
    {
        _logger.LogInformation($"CompleteTank called by hammaq. TankId: {request.TankId}, StationId: {request.StationId}");
        var result = await _tankHandler.HandleTankComplete(request);
        _logger.LogInformation($"Tank completed Successfully for TankId: {request.TankId}.");
        return Ok(result);
    }
}