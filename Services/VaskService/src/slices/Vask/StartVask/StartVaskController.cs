using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.VaskModels;

namespace VaskService.Slices.Vask.StartVask;

[ApiController]
[Route("api/[controller]")]
public class StartVaskController : ControllerBase
{
    private readonly ILogger<StartVaskController> _logger;
    private readonly StartVaskHandler _vaskService;

    public StartVaskController(ILogger<StartVaskController> logger, StartVaskHandler vaskService)
    {
        _logger = logger;
        _vaskService = vaskService;
    }

    [HttpPost("StartVask")]
    public async Task<IActionResult> StartVask([FromBody] VaskRequest request)
    {
        _logger.LogInformation($"StartVask called for user {request.UserId}, VaskType: {request.Vasktype}, stationId: {request.StationId}.");
        var result = await _vaskService.HandleStartVask(request);
        _logger.LogInformation($"Vask started with Successfully. VaskId: {result.VaskId} for user {request.UserId}.");
        return Ok(result);
    }
}