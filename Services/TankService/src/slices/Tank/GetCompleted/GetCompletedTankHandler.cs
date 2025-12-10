using Microsoft.EntityFrameworkCore;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.TankModels;

namespace TankService.Slices.Tank.GetCompleted;

public class GetTankCompleteTransactiondHandler
{
    private readonly ILogger<GetTankCompleteTransactiondHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    private readonly SharedDbContext _dbContext;

    public GetTankCompleteTransactiondHandler(ILogger<GetTankCompleteTransactiondHandler> logger, SharedDbContext dbContext, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<List<TankCompleteResponse>> GetTankCompletedTransactionAsync(int userId)
    {
        var completed = await _dbContext.TankCompleteResponses
         .Where(v => v.UserId == userId && v.CurrentStatus == "Completed")
         .ToListAsync();
        return completed!;
    }

    public void AddTransactionToDb(TankCompleteResponse completedTransaction)
    {
        _dbContext.TankCompleteResponses.Add(completedTransaction);
        _dbContext.SaveChanges();
    }
}