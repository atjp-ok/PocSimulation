using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.TankModels;

namespace BFF.NewBFF.Slices.Tank.CompleteTank;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class CompleteTankController : ControllerBase
{
    private readonly ILogger<CompleteTankController> _logger;
    private readonly CompleteTankHandler _completeTankHandler;

    public CompleteTankController(ILogger<CompleteTankController> logger, CompleteTankHandler completeTankHandler)
    {
        _logger = logger;
        _completeTankHandler = completeTankHandler;
    }

    [HttpPost("CompleteTank")]
    public async Task<IActionResult> TankComplete([FromBody] TankCompleteRequest request)
    {
        _logger.LogInformation("TankComplete called in BFF.");
        var result = await _completeTankHandler.HandleTankCompleteAsync(request);
        _logger.LogInformation("TankComplete completed in BFF.");
        return Ok(result);
    }
}