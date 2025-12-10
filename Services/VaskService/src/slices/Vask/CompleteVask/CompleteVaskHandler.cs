using Shared.SharedModels.UserModels;
using Shared.SharedModels.PSpModels;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.VaskModels;

namespace VaskService.Slices.CompleteVask;
public class CompleteVaskHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CompleteVaskHandler> _logger;
    private readonly SharedDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly string _userProfileServiceUrl;
    private readonly string _pspServiceUrl;

    public CompleteVaskHandler(ILogger<CompleteVaskHandler> logger, HttpClient httpClient, IConfiguration configuration, SharedDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _dbContext = dbContext;
        _userProfileServiceUrl = configuration["ServiceUrls:UserProfileService"] ?? string.Empty;
        _pspServiceUrl = configuration["ServiceUrls:PspService"] ?? string.Empty;
    }
    public async Task<VaskCompleteResponse> HandleCompleteVask(VaskCompleteRequest vaskComplete)
    {
        var vask = _dbContext.VaskStatusResponses.FirstOrDefault(t => t.VaskId == vaskComplete.VaskId);
        if (vask == null) throw new Exception("Vask not found");
        var UserId = vask.UserId;
        _logger.LogInformation($"Completing Vask for Vask ID: {vaskComplete.VaskId} for station {vaskComplete.StationId}, and for user ID: {UserId}");

        vask.CurrentStatus = "Completed";
        _dbContext.VaskStatusResponses.Update(vask);
        await  _dbContext.SaveChangesAsync();

        var userProfile = await _httpClient.GetFromJsonAsync<UserProfileModel>
        ($"{_userProfileServiceUrl}/api/UserProfile/{UserId}");
        if (userProfile == null) throw new Exception("User profile not found");

        vask.AmountToPay = vask.Vasktype == "Premium" ? 250 : 150; // Example pricing logic
        _dbContext.VaskStatusResponses.Update(vask);
        await _dbContext.SaveChangesAsync();
        var payment = new Request
        {
            UserId = UserId,
            Amount = vask.AmountToPay,
            ServiceId = vaskComplete.VaskId,
            ServiceType = "Vask"
        };

        var paymentResponse = await _httpClient.PostAsJsonAsync(
            $"{_pspServiceUrl}/api/Capture/Capture", payment);
        if (!paymentResponse.IsSuccessStatusCode) throw new Exception("Payment capture failed");
        _logger.LogInformation($"Payment captured for Vask ID: {vask.VaskId}, user ID: {UserId}");

        var result = new VaskCompleteResponse
        {
            VaskId = vask.VaskId,
            UserId = vask.UserId,
            StationId = vask.StationId,
            CurrentStatus = vask.CurrentStatus,
            Vasktype = vask.Vasktype,
            AmountToPay = vask.AmountToPay,
            Message = $"Vask Completed for User {UserId}. Amount to pay {vask.AmountToPay}kr."
        };
        _dbContext.VaskCompleteResponses.Add(result);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"Vask completed and recorded for Vask ID: {vask.VaskId}, user ID: {UserId}");
        return await Task.FromResult(result);
    }
}