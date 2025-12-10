using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.TankModels;

namespace TankService.Slices.Tank.GetCompleted;

[ApiController]
[Route("api/[controller]")]
public class GetCompletedTransactionController : ControllerBase 
{
    private readonly ILogger<GetCompletedTransactionController> _logger;
    private readonly GetTankCompleteTransactiondHandler _handler;

    public GetCompletedTransactionController(ILogger<GetCompletedTransactionController> logger, GetTankCompleteTransactiondHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    [HttpGet]
    [Route("GetCompletedTransaction/{userId}")]
    public async Task<ActionResult<List<TankCompleteResponse>>> GetCompletedTransaction(int userId)
    {
        _logger.LogInformation("Received request to get completed vask transaction.");
        var completedTransaction = await _handler.GetTankCompletedTransactionAsync(userId);
       if(completedTransaction == null || !completedTransaction.Any())
        {
            _logger.LogWarning($"No completed transaction found for vaskId: {userId}", userId);
            return Ok(new List<TankCompleteResponse>());
        }
        _logger.LogInformation($"Returning completed vask transaction for userId: {userId}", userId);
        return Ok(completedTransaction);
    }
}