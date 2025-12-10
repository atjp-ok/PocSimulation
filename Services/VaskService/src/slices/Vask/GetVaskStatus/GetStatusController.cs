using Microsoft.AspNetCore.Mvc;

namespace VaskService.Slices.GetStatusService;

[ApiController]
[Route("api/[controller]")]
public class GetStatusVaskController : ControllerBase
{
    private readonly ILogger<GetStatusVaskController> _logger;
    private readonly GetStatusVaskHandler _vaskService;

    public GetStatusVaskController(ILogger<GetStatusVaskController> logger, GetStatusVaskHandler vaskService)
    {
        _logger = logger;
        _vaskService = vaskService;

    }

    [HttpGet("GetStatusVask")]
    public async Task<IActionResult> GetVaskStatus(int vaskId)
    {
        _logger.LogInformation($"GetVaskStatus called for VaskId: {vaskId}.");
        var result = await _vaskService.HandleVaskStatus(vaskId);
        if (result == null)
        {
            _logger.LogInformation($"Vask not found for VaskId: {vaskId}.");
            return NotFound();
        }
        _logger.LogInformation($"Vask status retrieved for VaskId: {vaskId}. Status: {result.CurrentStatus}");
        return Ok(result);
    }

}