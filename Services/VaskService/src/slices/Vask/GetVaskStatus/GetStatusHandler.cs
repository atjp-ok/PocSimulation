using Shared.SharedModels.HammaqModels;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.VaskModels;

namespace VaskService.Slices.GetStatusService;
public class GetStatusVaskHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GetStatusVaskHandler> _logger;
    private readonly SharedDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly string _hammaqServiceUrl;

    public GetStatusVaskHandler(ILogger<GetStatusVaskHandler> logger, HttpClient httpClient, IConfiguration configuration, SharedDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _dbContext = dbContext;
        _hammaqServiceUrl = configuration["ServiceUrls:HammaqService"] ?? string.Empty;
    }

    public async Task<VaskStatusResponse> HandleVaskStatus(int VaskId)
    {
        _logger.LogInformation($"Fetching Vask status for Vask ID: {VaskId}");
        var vask = _dbContext.VaskStatusResponses.FirstOrDefault(t => t.VaskId == VaskId);
        if (vask == null)
        {
            _logger.LogWarning($"Vask not found for Vask ID: {VaskId}");
            return null!;
        }
        var hammaqStatus = await _httpClient.GetFromJsonAsync<HammaqResponse>
        ($"{_hammaqServiceUrl}/api/GetStatus/GetStatus?ServiceType=Vask&ServiceId={vask.VaskId}");
        if (hammaqStatus != null)
        {
            vask.CurrentStatus = hammaqStatus.Status;
            _dbContext.Update(vask);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Vask status for Vask ID: {VaskId} is {hammaqStatus.Status}");
        }
        return await Task.FromResult(vask);
    }
}