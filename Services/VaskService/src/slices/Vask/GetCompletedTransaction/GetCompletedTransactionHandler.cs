using Microsoft.EntityFrameworkCore;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.VaskModels;

namespace VaskService.Slices.Vask.GetCompleted;

public class GetCompletedTransactionHandler
{
    private readonly ILogger<GetCompletedTransactionHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    private readonly SharedDbContext _dbContext;

    public GetCompletedTransactionHandler(ILogger<GetCompletedTransactionHandler> logger, SharedDbContext dbContext, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<List<VaskCompleteResponse>> GetCompletedTransactionAsync(int userId)
    {
        var completed = await _dbContext.VaskCompleteResponses
       .Where(v => v.UserId == userId && v.CurrentStatus == "Completed")
       .ToListAsync();
        return completed!;
    }

    public void AddTransactionToDb(VaskCompleteResponse completedTransaction)
    {
        _dbContext.VaskCompleteResponses.Add(completedTransaction);
        _dbContext.SaveChanges();
    }
}