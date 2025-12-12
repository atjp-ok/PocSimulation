using HammaqService.slices.GetHammaqStatus;
using Shared.SharedModels.HammaqModels;
using Microsoft.AspNetCore.Mvc;

namespace HammaqService.Controllers.Hammaq;

[ApiController]
[Route("api/[controller]")]
public class GetStatusController : ControllerBase
{
    private readonly GetStatusHandler _hammaqHandler;
    private readonly ILogger<GetStatusController> _logger;

    public GetStatusController(ILogger<GetStatusController> logger, GetStatusHandler hammaqHandler)
    {
        _logger = logger;
        _hammaqHandler = hammaqHandler;
    }

    /// <summary>
    /// Gets the status of a Hammaq service request called by Vask and Tank services.
    /// </summary>
    /// <param name="ServiceType"></param>
    /// <param name="ServiceId"></param>
    /// <returns></returns>
    [HttpGet("GetStatus")]
    public async Task<IActionResult> GetStatus([FromQuery] string ServiceType, [FromQuery] int ServiceId)
    {
        _logger.LogInformation($"GetStatus called for {ServiceType} (Id: {ServiceId}).");
        var request = new HammaqRequest
        {
            ServiceType = ServiceType,
            ServiceId = ServiceId
        };
        var result = await _hammaqHandler.HandleStatus(request);
        _logger.LogInformation($"Hammaq service status retrieved for {ServiceType} with ID {ServiceId}: {result.Status}");

        return Ok(result);
    }
}