using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.VaskModels;

namespace BFF.NewBFF.Slices.Vask.CompleteVask;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class CompleteVaskController : ControllerBase
{
    private readonly ILogger<CompleteVaskController> _logger;
    private readonly CompleteVaskHandler _completeVaskHandler;

    public CompleteVaskController(ILogger<CompleteVaskController> logger, CompleteVaskHandler completeVaskHandler)
    {
        _logger = logger;
        _completeVaskHandler = completeVaskHandler;
    }

    /// <summary>
    /// Completes a vask session
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("CompleteVask")]
    public async Task<IActionResult> VaskComplete([FromBody] VaskCompleteRequest request)
    {
        _logger.LogInformation("VaskComplete called in BFF.");
        var result = await _completeVaskHandler.HandleCompleteVaskAsync(request);
        _logger.LogInformation("VaskComplete completed in BFF.");
        return Ok(result);
    }

}