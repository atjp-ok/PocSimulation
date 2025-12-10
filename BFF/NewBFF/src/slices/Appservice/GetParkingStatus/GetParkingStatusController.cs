using Microsoft.AspNetCore.Mvc;

namespace BFF.NewBFF.Slices.Appservice.GetParkingStatus;

[ApiController]
[Route("api/[controller]")]
public class GetParkingStatusController : ControllerBase
{
    private readonly ILogger<GetParkingStatusController> _logger;
    private readonly GetParkingStatusHandler _getParkingStatusHandler;

    public GetParkingStatusController(ILogger<GetParkingStatusController> logger, GetParkingStatusHandler getParkingStatusHandler)
    {
        _logger = logger;
        _getParkingStatusHandler = getParkingStatusHandler;
    }

    [HttpGet("GetParkingStatus/{parkingId}")]
    public async Task<IActionResult> GetParkingStatus(int parkingId)
    {
        _logger.LogInformation("GetParkingStatus called in BFF.");
        var result = await _getParkingStatusHandler.HandleParkingStatusAsync(parkingId);
        _logger.LogInformation("GetParkingStatus completed in BFF.");
        return Ok(result);
    }

}