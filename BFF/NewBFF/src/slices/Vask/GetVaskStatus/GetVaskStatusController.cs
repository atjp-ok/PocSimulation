using Microsoft.AspNetCore.Mvc;

namespace BFF.NewBFF.Slices.Vask.GetVaskStatus;

[ApiController]
[Route("api/[controller]")]
public class GetStatusVaskController : ControllerBase
{
    private readonly ILogger<GetStatusVaskController> _logger;
    private readonly GetStatusVaskHandler _getVaskStatusHandler;

    public GetStatusVaskController(ILogger<GetStatusVaskController> logger, GetStatusVaskHandler getVaskStatusHandler)
    {
        _logger = logger;
        _getVaskStatusHandler = getVaskStatusHandler;
    }
    [HttpGet("GetStatusVask")]
    public async Task<IActionResult> HandleVaskStatusAsync(int vaskId)
    {
        _logger.LogInformation("GetVaskStatus called in BFF.");
        var result = await _getVaskStatusHandler.GetVaskStatus(vaskId);
        _logger.LogInformation("GetVaskStatus completed in BFF.");
        return Ok(result);
    }

}