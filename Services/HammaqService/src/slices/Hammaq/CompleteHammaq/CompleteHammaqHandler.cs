using Shared.SharedModels.TankModels;
using Shared.SharedModels.VaskModels;
using Shared.SharedModels.HammaqModels;
using Shared.SharedModels.SharedDbContext;

namespace HammaqService.slices.Hammaq.CompleteHammaq;
public class CompleteHammaqHandler
{
    private readonly ILogger<CompleteHammaqHandler> _logger;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly IConfiguration _configuration;
    private readonly SharedDbContext _dbContext;
    private readonly string _tankServiceUrl;
    private readonly string _vaskServiceUrl;
    public CompleteHammaqHandler(ILogger<CompleteHammaqHandler> logger, IConfiguration configuration, HttpClient httpClient, SharedDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _tankServiceUrl = configuration["ServiceUrls:TankService"] ?? string.Empty;
        _vaskServiceUrl = configuration["ServiceUrls:VaskService"] ?? string.Empty;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Completes the Hammaq service for a request and notifies Vask and Tank services.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<HammaqResponse> HandleComplete(HammaqRequest request)
    {
        var status = _dbContext.HammaqResponses.FirstOrDefault(s => s.ServiceId == request.ServiceId && s.ServiceType == request.ServiceType);
        if (status != null)
        {
            status.Status = "Completed";
            _logger.LogInformation($"Hammaq service completed for {request.ServiceType} with ID {request.ServiceId} is done.");

            if (request.ServiceType == "Tank")
            {
                await _httpClient.PostAsJsonAsync($"{_tankServiceUrl}/api/CompleteTank/CompleteTank", new TankCompleteRequest
                {
                    TankId = request.ServiceId,
                    StationId = request.StationId,
                    ActualLiters = 30
                });
            }
            else if (request.ServiceType == "Vask")
            {
                await _httpClient.PostAsJsonAsync($"{_vaskServiceUrl}/api/CompleteVask/CompleteVask", new VaskCompleteRequest
                {
                    VaskId = request.ServiceId,
                    StationId = request.StationId,
                });
            }
        }
        else
        {
            _logger.LogInformation($"Hammaq service completion attempted for {request.ServiceType} with ID {request.ServiceId}, but not found.");
            status = new HammaqResponse
            {
                ServiceId = request.ServiceId,
                ServiceType = request.ServiceType,
                Status = "NotFound"
            };
        }
        return status;
    }


}