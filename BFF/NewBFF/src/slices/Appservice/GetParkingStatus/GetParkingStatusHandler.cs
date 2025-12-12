using Shared.SharedModels.AppserviceModels;

namespace BFF.NewBFF.Slices.Appservice.GetParkingStatus;
public class GetParkingStatusHandler
{
    private readonly ILogger<GetParkingStatusHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _appServiceUrl;
    private readonly IConfiguration _configuration;

    public GetParkingStatusHandler(ILogger<GetParkingStatusHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _appServiceUrl = configuration["ServiceUrls:AppService"] ?? string.Empty;
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the status of a parking session for a given parking ID and returns the result.
    /// </summary>
    /// <param name="parkingId"></param>
    /// <returns></returns>
    public async Task<ParkingResponse> HandleParkingStatusAsync(int parkingId)
    {
        _logger.LogInformation("Calling AppService to get parking status.");
        var response = await _httpClient.GetAsync($"{_appServiceUrl}/api/AppService/GetParkingStatus/{parkingId}");
        response.EnsureSuccessStatusCode();
        var parkingStatusResult = await response.Content.ReadFromJsonAsync<ParkingResponse>();
        _logger.LogInformation("Received response from AppService for getting parking status.");
        return parkingStatusResult!;
    }
}