using Shared.SharedModels.UserModels;
using Shared.SharedModels.PSpModels;
using Shared.SharedModels.HammaqModels;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.VaskModels;

namespace VaskService.Slices.Vask.StartVask;

public class StartVaskHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StartVaskHandler> _logger;
    private static int _VaskId = 1;
    private readonly IConfiguration _configuration;
    private readonly string _userProfileServiceUrl;
    private readonly string _pspServiceUrl;
    private readonly string _hammaqServiceUrl;
    private readonly SharedDbContext _dbContext;

    public StartVaskHandler(ILogger<StartVaskHandler> logger, HttpClient httpClient, IConfiguration configuration, SharedDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _dbContext = dbContext;
        _userProfileServiceUrl = configuration["ServiceUrls:UserProfileService"] ?? string.Empty;
        _pspServiceUrl = configuration["ServiceUrls:PspService"] ?? string.Empty;
        _hammaqServiceUrl = configuration["ServiceUrls:HammaqService"] ?? string.Empty;
    }

    public async Task<VaskStatusResponse> HandleStartVask(VaskRequest request)
    {
        _logger.LogInformation($"Starting Vask for user ID: {request.UserId} at station ID: {request.StationId}");
        var profileResponse = await _httpClient.GetFromJsonAsync<UserProfileModel>
        ($"{_userProfileServiceUrl}/api/UserProfile/{request.UserId}");
        if (profileResponse == null)
        {
            _logger.LogWarning($"User profile not found for user ID: {request.UserId}");
            throw new Exception("User profile not found");
        }
        var amount = request.Vasktype == "Premium" ? 250 : 150; // Example pricing logic
        var payment = new Request
        {
            UserId = request.UserId,
            Amount = amount, 
            ServiceId = _VaskId,
            ServiceType = "Vask"
        };
        var paymentResponse = await _httpClient.PostAsJsonAsync(
            $"{_pspServiceUrl}/api/Reserve/Reserve", payment);

        if (!paymentResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Payment reservation failed for user ID: {request.UserId}, Vask ID: {_VaskId}");
            throw new Exception("Payment reservation failed");
        }
        var vask = new VaskStatusResponse
        {
            VaskId = _VaskId++,
            UserId = request.UserId,
            StationId = request.StationId,
            CurrentStatus = "InProgress",
            Vasktype = request.Vasktype,
            AmountToPay = amount, 
            Message = "Vask started successfully."
        };
        _dbContext.VaskStatusResponses.Add(vask);
        await _dbContext.SaveChangesAsync();

        var hammaqRequest = new HammaqRequest
        {
            ServiceId = vask.VaskId,
            ServiceType = "Vask",
            StationId = request.StationId
        };
        var hammaqResponse = await _httpClient.PostAsJsonAsync(
            $"{_hammaqServiceUrl}/api/StartHammaq/Start", hammaqRequest);

        if (!hammaqResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Hammaq start failed for Vask ID: {vask.VaskId} at station ID: {request.StationId}");
            throw new Exception("Hammaq start failed");
        }
         _logger.LogInformation($"Vask started with ID: {vask.VaskId}, for user ID: {request.UserId}");
        return await Task.FromResult(vask);
    }
}
