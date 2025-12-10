using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.PSpModels;

namespace PspService.slices.Reserve;

[ApiController]
[Route("api/[controller]")]
public class ReserveController : ControllerBase
{
    private readonly ILogger<ReserveController> _logger;
    private readonly ReserveHandler _reserveHandler;
    public ReserveController(ILogger<ReserveController> logger, ReserveHandler reserveHandler)
    {
        _logger = logger;
        _reserveHandler = reserveHandler;
    }

    [HttpPost("Reserve")]
    public async Task<IActionResult> Reserve([FromBody] Request payment)
    {

        var result = await _reserveHandler.HandleReserve(payment);

        return Ok(new
        {
            Message = "Payment Reserved Successfully",
            PaymentStatus = result
        });
    }
}