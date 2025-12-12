using Shared.SharedModels.PSpModels;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.UserModels;
using Shared.SharedModels.TankModels;
using Shared.SharedModels.HammaqModels;

namespace TankService.slices.Tank.StartTank;
public class StartTankHandler
{
    private readonly ILogger<StartTankHandler> _logger;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly SharedDbContext _dbContext;
    private static int _tankId = 1;
    private readonly IConfiguration _configuration;
    private readonly string _userProfileServiceUrl;
    private readonly string _pspServiceUrl;
    private readonly string _hammaqServiceUrl;

    public StartTankHandler(ILogger<StartTankHandler> logger, HttpClient httpClient, IConfiguration configuration, SharedDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _dbContext = dbContext;
        _userProfileServiceUrl = configuration["ServiceUrls:UserProfileService"] ?? string.Empty;
        _pspServiceUrl = configuration["ServiceUrls:PspService"] ?? string.Empty;
        _hammaqServiceUrl = configuration["ServiceUrls:HammaqService"] ?? string.Empty;
    }

    /// <summary>
    /// Starts a tanking session, reserves payment, and calls hammaq to start tanking
    /// </summary>
    public async Task<TankStatusResponse> HandleStartTank(StartTankRequest request)
    {
        _logger.LogInformation($"Starting Tank for user ID: {request.UserId} at station ID: {request.StationId}, FuelType: {request.Fueltype}");
        var profileResponse = await _httpClient.GetFromJsonAsync<UserProfileModel>
        ($"{_userProfileServiceUrl}/api/UserProfile/{request.UserId}");
        if (profileResponse == null)
        {
            _logger.LogWarning($"User profile not found for user ID: {request.UserId}");
            throw new Exception("User profile not found");
        }
        var amount = 800; // Reserve amount for tanking
        var payment = new Request
        {
            UserId = request.UserId,
            Amount = amount,
            ServiceId = _tankId,
            ServiceType = "Tank"
        };
        var reserveResponse = await _httpClient.PostAsJsonAsync(
            $"{_pspServiceUrl}/api/Reserve/Reserve", payment);

        if (!reserveResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Payment reservation failed for TankId: {_tankId}, user ID: {request.UserId}");
            throw new Exception("Payment reservation failed");
        }
        _logger.LogInformation($"Payment reserved for TankId: {_tankId}, user ID: {request.UserId}");

        var tank = new TankStatusResponse
        {
            TankId = _tankId++,
            CurrentStatus = "InProgress",
            Fueltype = request.Fueltype,
            StationId = request.StationId,
            UserId = request.UserId
        };
        _dbContext.TankResponses.Add(tank);
        await _dbContext.SaveChangesAsync();

        var hammaqRequest = new HammaqRequest
        {
            ServiceId = tank.TankId,
            ServiceType = "Tank",
            StationId = request.StationId
        };
        await _httpClient.PostAsJsonAsync(
            $"{_hammaqServiceUrl}/api/StartHammaq/Start", hammaqRequest);
        _logger.LogInformation($"Tank started successfully with TankId: {tank.TankId}, Fueltype: {tank.Fueltype}.");
        return await Task.FromResult(tank);
    }
}
