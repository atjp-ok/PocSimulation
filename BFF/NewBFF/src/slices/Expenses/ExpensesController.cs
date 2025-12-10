using Microsoft.AspNetCore.Mvc;

namespace NewBFF.Slices.Expenses;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly ILogger<ExpenseController> _logger;
    private readonly ExpenseHandler _expenseHandler;
    public ExpenseController(ExpenseHandler expenseHandler, ILogger<ExpenseController> logger)
    {
        _expenseHandler = expenseHandler;
        _logger = logger;
    }

    [HttpGet("GetExpenses/{userId}")]
    public async Task<IActionResult> GetExpenses(int userId)
    {
        _logger.LogInformation("GetExpenses called in BFF.");
        var result = await _expenseHandler.HandleGetExpensesAsync(userId);
        _logger.LogInformation("GetExpenses completed in BFF.");
        return Ok(result);
    }

}
