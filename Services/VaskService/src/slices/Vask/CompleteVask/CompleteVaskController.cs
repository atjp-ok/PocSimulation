using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.VaskModels;
using VaskService.Slices.CompleteVask;

namespace VaskService.Slices.CompleteVaskController;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class CompleteVaskController : ControllerBase
{
    private readonly ILogger<CompleteVaskController> _logger;
    private readonly CompleteVaskHandler _vaskHandler;

    public CompleteVaskController(ILogger<CompleteVaskController> logger, CompleteVaskHandler vaskHandler)
    {
        _logger = logger;
        _vaskHandler = vaskHandler;

    }

    [HttpPost("CompleteVask")]
    public async Task<IActionResult> VaskComplete([FromBody] VaskCompleteRequest request)
    {
        _logger.LogInformation($"CompleteVask called for VaskId: {request.VaskId}, userId: {request.UserId}, StationId: {request.StationId}.");
        var result = await _vaskHandler.HandleCompleteVask(request);
        if (result.CurrentStatus == "NotFound")
        {
            _logger.LogInformation($"Vask not found VaskId {request.VaskId}.");
            return NotFound();
        }
        _logger.LogInformation($"Vask completed Successfully for VaskId: {request.VaskId} for userId: {request.UserId}.");
        return Ok(result);
    }
}