using Shared.SharedModels.TankModels;
using Shared.SharedModels.VaskModels;

namespace NewBFF.Slices.Expenses;
public class ExpenseHandler
{
    private readonly ILogger<ExpenseHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _tankServiceUrl;
    private readonly string _vaskServiceUrl;

    public ExpenseHandler(ILogger<ExpenseHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _tankServiceUrl = configuration["ServiceUrls:TankService"] ?? string.Empty;
        _vaskServiceUrl = configuration["ServiceUrls:VaskService"] ?? string.Empty;
    }

    /// <summary>
    /// Gets expenses for a given user ID and calls TankService and VaskService to aggregate expenses.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<ExpensesDto> HandleGetExpensesAsync(int userId)
    {
        _logger.LogInformation("Fetching expenses from TankService, VaskService, and AppserviceService.");

        var tankTransactions = await _httpClient.GetFromJsonAsync<List<TankCompleteResponse>>($"{_tankServiceUrl}/api/GetCompletedTransaction/GetCompletedTransaction/{userId}");
        if (tankTransactions == null)
            tankTransactions = new List<TankCompleteResponse>();

        var vaskResponse = await _httpClient.GetFromJsonAsync<List<VaskCompleteResponse>>($"{_vaskServiceUrl}/api/GetCompletedTransaction/GetCompletedTransaction/{userId}");
        if (vaskResponse == null)
            vaskResponse = new List<VaskCompleteResponse>();

        double totalVask = vaskResponse.Sum(v => v.AmountToPay);
        double totalTank = tankTransactions.Sum(t => t.AmountCharged);

        var expensesDto = new ExpensesDto
        {
            UserId = userId,
            TotalVask = totalVask,
            TotalTank = totalTank,
            TotalExpenses = totalVask + totalTank,
            ExpenseBreakdown = new List<ExpenseBreakDto>()
            {
                new ExpenseBreakDto{Category = "Tank", Amount = totalTank},
                new ExpenseBreakDto{Category = "Vask", Amount = totalVask}
            }
        };
        return expensesDto;
    }
}