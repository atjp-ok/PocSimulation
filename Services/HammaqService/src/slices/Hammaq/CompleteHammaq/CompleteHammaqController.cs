using HammaqService.slices.Hammaq.CompleteHammaq;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.HammaqModels;

namespace HammaqService.Controllers.Hammaq;

[ApiController]
[Route("api/[controller]")]
public class CompleteHammaqController : ControllerBase
{
    private readonly CompleteHammaqHandler _hammaqHandler;
    private readonly ILogger<CompleteHammaqController> _logger;

    public CompleteHammaqController(ILogger<CompleteHammaqController> logger, CompleteHammaqHandler hammaqHandler)
    {
        _logger = logger;
        _hammaqHandler = hammaqHandler;
    }

    [HttpPost("Complete")]
    public async Task<IActionResult> Complete([FromBody] HammaqRequest request)
    {
        _logger.LogInformation($"Complete called for {request.ServiceType} (Id: {request.ServiceId}).");
        var result = await _hammaqHandler.HandleComplete(request);
        return Ok(result);

    }
}