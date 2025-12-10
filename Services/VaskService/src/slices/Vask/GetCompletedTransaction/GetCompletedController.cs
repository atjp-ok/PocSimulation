using Microsoft.AspNetCore.Mvc;
using Shared.SharedModels.VaskModels;

namespace VaskService.Slices.Vask.GetCompleted;

[ApiController]
[Route("api/[controller]")]
public class GetCompletedTransactionController : ControllerBase
{
    private readonly ILogger<GetCompletedTransactionController> _logger;
    private readonly GetCompletedTransactionHandler _handler;

    public GetCompletedTransactionController(ILogger<GetCompletedTransactionController> logger, GetCompletedTransactionHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    [HttpGet]
    [Route("GetCompletedTransaction/{userId}")]
    public async Task<ActionResult<List<VaskCompleteResponse>>> GetCompletedTransaction(int userId)
    {
        _logger.LogInformation("Received request to get completed vask transaction.");
        var completedTransaction = await _handler.GetCompletedTransactionAsync(userId);
        if (completedTransaction == null || !completedTransaction.Any())
        {
            _logger.LogWarning($"No completed transaction found for vaskId: {userId}", userId);
            return Ok(new List<VaskCompleteResponse>());
        }
        _logger.LogInformation($"Returning completed vask transaction for vaskId: {userId}", userId);
        return Ok(completedTransaction);
    }
}