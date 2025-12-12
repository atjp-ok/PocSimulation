using Shared.SharedModels.PSpModels;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.UserModels;
using Shared.SharedModels.AppserviceModels;

namespace AppService.Services;
public class AppService : IAppService
{
    private readonly ILogger<AppService> _logger;
    private readonly SharedDbContext _dbContext;
    private HttpClient _httpClient;
    private static int _parkingId = 1;
    private readonly IConfiguration _configuration;
    private readonly string _userProfileServiceUrl;
    private readonly string _pspServiceUrl;
    public AppService(ILogger<AppService> logger, HttpClient httpClient, IConfiguration configuration, SharedDbContext dbContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
        _userProfileServiceUrl = configuration["ServiceUrls:UserProfileService"] ?? string.Empty;
        _pspServiceUrl = configuration["ServiceUrls:PspService"] ?? string.Empty;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Starts a parking session and reserves payment
    /// and returns parking details.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ParkingResponse> StartParkingAsync(ParkingRequest request)
    {
        var parkingId = _parkingId++;
        _logger.LogInformation($"Parking started for UserID: {request.UserId} at {request.Location}, ParkingId {parkingId}.");
        var userProfile = await _httpClient.GetFromJsonAsync<UserProfileModel>
            ($"{_userProfileServiceUrl}/api/UserProfile/{request.UserId}");
        if (userProfile == null)
        {
            _logger.LogWarning($"User profile not found for UserID: {request.UserId}");
            throw new Exception("User profile not found");
        }
        var payment = new Request
        {
            UserId = request.UserId,
            Amount = 50, // Example pricing logic
            ServiceId = parkingId,
            ServiceType = "Parking"
        };

        var paymentResponse = await _httpClient.PostAsJsonAsync(
            $"{_pspServiceUrl}/api/Reserve/Reserve", payment);
        if (!paymentResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Payment reservation failed for ParkingId: {parkingId}, UserID: {request.UserId}");
            throw new Exception("Payment reservation failed");
        }

        var responseDto = new ParkingResponse
        {
            ParkingId = parkingId,
            Status = "Active",
            StartTime = DateTime.UtcNow,
            UserId = request.UserId,
        };
        _dbContext.ParkingStatusResponses.Add(responseDto);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"User {request.UserId} Started parking {parkingId}, at location: {request.Location}.");
        return await Task.FromResult(responseDto);
    }

    /// <summary>
    /// Stops a parking session, captures payment,
    /// and returns parking details.
    /// </summary>
    /// <param name="parkingId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<StopParkingResponse?> StopParkingAsync(int parkingId)
    {
        var parking = _dbContext.ParkingStatusResponses.FirstOrDefault(p => p.ParkingId == parkingId);
        if (parking == null)
        {
            _logger.LogInformation($"Parking not found. ParkingId: {parkingId}.");
            return null;
        }
        if (parking.StartTime == null)
        {
            _logger.LogWarning($"Invalid start time for ParkingId: {parking.ParkingId}.");
            throw new Exception("Invalid start time");
        }
        var endTime = DateTime.UtcNow;
        var durationHour = (endTime - parking.StartTime.Value).TotalHours;
        durationHour = Math.Max(durationHour, 1);

        var ratePerHour = 20;
        var amountToCharge = durationHour * ratePerHour;

        var payment = new Request
        {
            UserId = parking.UserId,
            Amount = amountToCharge,
            ServiceId = parking.ParkingId,
            ServiceType = "Parking"
        };
        var paymentResponse = await _httpClient.PostAsJsonAsync(
            $"{_pspServiceUrl}/api/Capture/Capture", payment);
        if (!paymentResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Payment capture failed for ParkingId: {parking.ParkingId}, user ID: {parking.UserId}");
            throw new Exception("Payment capture failed");
        }
        parking.Status = "Completed";
        _logger.LogInformation($"Parking stopped for ParkingId: {parking.ParkingId}, UserID: {parking.UserId}. Amount to pay: {amountToCharge} kr.");

        return new StopParkingResponse
        {
            ParkingId = parking.ParkingId,
            Status = parking.Status,
            StartTime = parking.StartTime,
            EndTime = endTime,
            AmountToPay = amountToCharge,
            Message = $"Parking stopped for ParkingId: {parking.ParkingId}, UserID: {parking.UserId}. Amount to pay: {amountToCharge} kr."
        };
    }


    /// <summary>
    /// Gets the status of a parking session.
    /// and returns parking details.
    /// </summary>
    /// <param name="parkingId"></param>
    /// <returns></returns>
    public async Task<ParkingResponse?> GetParkingStatusAsync(int parkingId)
    {
        var status = _dbContext.ParkingStatusResponses.FirstOrDefault(p => p.ParkingId == parkingId);
        if (status != null)
            _logger.LogInformation($"Retrieved parking status for ParkingId: {status.ParkingId}, Status: {status.Status}.");
        else
            _logger.LogInformation($"Parking status not found for ParkingId: {parkingId}.");
        return await Task.FromResult(status!);
    }
}