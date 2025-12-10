using AppService.Services;
using Shared.SharedModels.AppserviceModels;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppServiceController : ControllerBase
{
    private readonly IAppService _appService;
    private readonly ILogger<AppServiceController> _logger;
    public AppServiceController(IAppService appService, ILogger<AppServiceController> logger)
    {
        _appService = appService;
        _logger = logger;
    }

    [HttpPost("StartParking")]
    public async Task<ActionResult> StartParking([FromBody] ParkingRequest request)
    {
        _logger.LogInformation("StartParking called.");
        var result = await _appService.StartParkingAsync(request);
        return Ok(result);
    }

    [HttpPost("StopParking/{parkingId}")]
    public async Task<ActionResult> StopParking(int parkingId)
    {
        _logger.LogInformation("StopParking called.");
        var result = await _appService.StopParkingAsync(parkingId);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet("GetParkingStatus/{parkingId}")]
    public async Task<IActionResult> GetParkingStatus(int parkingId)
    {
        _logger.LogInformation("GetParkingStatus called.");
        var result = await _appService.GetParkingStatusAsync(parkingId);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

}