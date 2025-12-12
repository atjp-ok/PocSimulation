using Shared.SharedModels.AppserviceModels;

namespace BFF.NewBFF.Slices.Appservice.StartParking;
public class StartParkingHandler
{
    private readonly ILogger<StartParkingHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _appServiceUrl;
    private readonly IConfiguration _configuration;

    public StartParkingHandler(ILogger<StartParkingHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _appServiceUrl = configuration["ServiceUrls:AppService"] ?? string.Empty;
        _configuration = configuration;
    }

    /// <summary>
    /// Starts a parking session and returns the result.
    /// </summary>
    public async Task<ParkingResponse> StartParkingAsync(ParkingRequest request)
    {
        _logger.LogInformation("Calling AppService to start parking.");
        var response = await _httpClient.PostAsJsonAsync($"{_appServiceUrl}/api/AppService/StartParking", request);
        response.EnsureSuccessStatusCode();
        var parkingResult = await response.Content.ReadFromJsonAsync<ParkingResponse>();
        _logger.LogInformation("Received response from AppService for starting parking.");
        return parkingResult!;
    }
}