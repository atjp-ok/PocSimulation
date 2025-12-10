using Shared.SharedModels.HammaqModels;
using Shared.SharedModels.SharedDbContext;

namespace HammaqService.slices.GetHammaqStatus;
public class GetStatusHandler
{
    private readonly ILogger<GetStatusHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly SharedDbContext _dbContext;
    public GetStatusHandler(ILogger<GetStatusHandler> logger, IConfiguration configuration, HttpClient httpClient, SharedDbContext dbContext)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public Task<HammaqResponse> HandleStatus(HammaqRequest request)
    {
        var status = _dbContext.HammaqResponses.FirstOrDefault(s => s.ServiceId == request.ServiceId && s.ServiceType == request.ServiceType);
        if (status == null)
        {
            status = new HammaqResponse
            {
                ServiceId = request.ServiceId,
                ServiceType = request.ServiceType,
                Status = "NotFound"
            };
        }
        _logger.LogInformation($"Hammaq service status retrieved for {request.ServiceType} with ID {request.ServiceId}: {status.Status}");
        return Task.FromResult(status);

    }



}