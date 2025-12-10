using Shared.SharedModels.PSpModels;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.TankModels;
using Shared.SharedModels.UserModels;

namespace TankService.slices.CompleteTank;
public class CompleteTankHandler
{
    private readonly ILogger<CompleteTankHandler> _logger;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly SharedDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly string _userProfileServiceUrl;
    private readonly string _pspServiceUrl;

    public CompleteTankHandler(ILogger<CompleteTankHandler> logger, HttpClient httpClient, IConfiguration configuration, SharedDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _dbContext = dbContext;
        _userProfileServiceUrl = configuration["ServiceUrls:UserProfileService"] ?? string.Empty;
        _pspServiceUrl = configuration["ServiceUrls:PspService"] ?? string.Empty;
    }

    public async Task<TankCompleteResponse> HandleTankComplete(TankCompleteRequest tankComplete)
    {
        var tank = _dbContext.TankResponses.FirstOrDefault(t => t.TankId == tankComplete.TankId);
        if (tank == null) throw new Exception("Tank not found");
        var UserId = tank.UserId;
        _logger.LogInformation($"Completing Tank for TankId: {tankComplete.TankId}, for user {UserId}, at station ID: {tankComplete.StationId}.");

        tank.CurrentStatus = "Completed";
        _dbContext.TankResponses.Update(tank);
        await _dbContext.SaveChangesAsync();

        var userResponse = await _httpClient.GetFromJsonAsync<UserProfileModel>
        ($"{_userProfileServiceUrl}/api/UserProfile/{UserId}");
        if (userResponse == null) throw new Exception("User not found");

        double pricePerLiter = tank.Fueltype == "diesel" ? 13.0 : 13.7; // Example pricing logic
        double amountToCharge = tankComplete.ActualLiters * pricePerLiter;
        _dbContext.TankResponses.Update(tank);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Amount to charge calculated: {amountToCharge} for {tankComplete.ActualLiters} liters.");
        var payment = new Request
        {
            UserId = UserId,
            ServiceId = tankComplete.TankId,
            ServiceType = "Tank",
            Amount = amountToCharge,
        };
        var paymentResponse = await _httpClient.PostAsJsonAsync(
            $"{_pspServiceUrl}/api/Capture/Capture", payment);
        if (!paymentResponse.IsSuccessStatusCode) throw new Exception("Payment capture failed");
        _logger.LogInformation($"Payment captured successfully for UserId: {UserId}, TankId: {tank.TankId}, Amount: {amountToCharge}.");

        var result = new TankCompleteResponse
        {
            TankId = tank.TankId,
            CurrentStatus = tank.CurrentStatus,
            UserId = tank.UserId,
            AmountLiters = tankComplete.ActualLiters,
            Fueltype = tank.Fueltype,
            AmountCharged = amountToCharge,
            StationId = tankComplete.StationId,
            Message = $"Your tanking is complete. You have filled {tankComplete.ActualLiters} liters of {tank.Fueltype}. Amount charged: {amountToCharge.ToString("F2")} kr."
        };
        _dbContext.TankCompleteResponses.Add(result);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Your tanking is complete. You have filled {tankComplete.ActualLiters} liters of {tank.Fueltype}. Amount charged: {amountToCharge.ToString("F2")} kr.");
        return await Task.FromResult(result);
    }

}



