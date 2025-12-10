using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.PSpModels;

namespace PspService.slices.Capture;

[ApiController]
[Route("api/[controller]")]
public class CaptureController : ControllerBase
{
    private readonly ILogger<CaptureController> _logger;
    private readonly CaptureHandler _captureHandler;
    public CaptureController(ILogger<CaptureController> logger, CaptureHandler captureHandler)
    {
        _logger = logger;
        _captureHandler = captureHandler;
    }

    [HttpPost("Capture")]
    public async Task<IActionResult> Capture([FromBody] Request payment)
    {
        _logger.LogInformation($"Capture called for user {payment.UserId}, service {payment.ServiceType} ({payment.ServiceId}).");
        var result = await _captureHandler.HandleCapture(payment);

        return Ok(new
        {
            Message = "Payment Capture Successfully",
            PaymentStatus = result
        });
    }

}