using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.AppserviceModels;

namespace BFF.NewBFF.Slices.Appservice.StartParking;

[ApiController]
[Route("api/[controller]")]
public class StartParkingController : ControllerBase
{
    private readonly ILogger<StartParkingController> _logger;
    private readonly StartParkingHandler _startParkingHandler;

    public StartParkingController(ILogger<StartParkingController> logger,  StartParkingHandler startParkingHandler)
    {
        _logger = logger;
        _startParkingHandler = startParkingHandler;
    }

    [HttpPost("StartParking")]
    public async Task<IActionResult> StartParking([FromBody] ParkingRequest request)
    {
        _logger.LogInformation("StartParking called in BFF.");
        var result = await _startParkingHandler.StartParkingAsync(request);
        _logger.LogInformation("StartParking completed in BFF.");
        return Ok(result);
    }
}