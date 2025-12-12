using Microsoft.AspNetCore.Mvc;
using HammaqService.slices.StartHammaq;
using Shared.SharedModels.HammaqModels;

namespace HammaqService.slices.Hammaq.StartHammaq;

[ApiController]
[Route("api/[controller]")]
public class StartHammaqController : ControllerBase
{
    private readonly StartHammaqHandler _hammaqHandler;
    private readonly ILogger<StartHammaqController> _logger;

    public StartHammaqController(ILogger<StartHammaqController> logger, StartHammaqHandler hammaqHandler)
    {
        _logger = logger;
        _hammaqHandler = hammaqHandler;
    }

    /// <summary>
    /// Starts the Hammaq service for a request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Start")]
    public async Task<IActionResult> Start([FromBody] HammaqRequest request)
    {
        _logger.LogInformation($"Start called from station {request.StationId} for servicetype {request.ServiceType} Id: {request.ServiceId}.");
        var result = await _hammaqHandler.HandleStart(request);
        _logger.LogInformation($"Station is in progress.");
        return Ok(result);

    }
}