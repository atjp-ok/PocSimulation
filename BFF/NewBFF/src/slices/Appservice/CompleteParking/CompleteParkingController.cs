
using Microsoft.AspNetCore.Mvc;

namespace BFF.NewBFF.Slices.Appservice.CompleteParking;

[ApiController]
[Route("api/[controller]")]
public class CompleteParkingController : ControllerBase
{
    private readonly ILogger<CompleteParkingController> _logger;
    private readonly CompleteParkingHandler _completeParkingHandler;

    public CompleteParkingController(ILogger<CompleteParkingController> logger, CompleteParkingHandler completeParkingHandler)
    {
        _logger = logger;
        _completeParkingHandler = completeParkingHandler;
    }

    /// <summary>
    /// Stops the parking session for a given parking ID.
    /// </summary>
    /// <param name="parkingId"></param>
    /// <returns></returns>
    [HttpPost("StopParking/{parkingId}")]
    public async Task<IActionResult> StopParking(int parkingId)
    {
        _logger.LogInformation("StopParking called in BFF.");
        var result = await _completeParkingHandler.HandleCompleteParkingAsync(parkingId);
        _logger.LogInformation("StopParking completed in BFF.");
        return Ok(result);
    }

}