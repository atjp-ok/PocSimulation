using Shared.SharedModels.AppserviceModels;

namespace BFF.NewBFF.Slices.Appservice.CompleteParking;
public class CompleteParkingHandler
{
    private readonly ILogger<CompleteParkingHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _appServiceUrl;
    private readonly IConfiguration _configuration;

    public CompleteParkingHandler(ILogger<CompleteParkingHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _appServiceUrl = configuration["ServiceUrls:AppService"] ?? string.Empty;
        _configuration = configuration;
    }

    /// <summary>
    /// Stops the parking session for a given parking ID and returns the result.
    /// </summary>
    /// <param name="parkingId"></param>
    /// <returns></returns>
    public async Task<StopParkingResponse> HandleCompleteParkingAsync(int parkingId)
    {
        _logger.LogInformation("Calling AppService to stop parking.");
        var response = await _httpClient.PostAsync($"{_appServiceUrl}/api/AppService/StopParking/{parkingId}", null);
        response.EnsureSuccessStatusCode();
        var stopResult = await response.Content.ReadFromJsonAsync<StopParkingResponse>();
        _logger.LogInformation("Received response from AppService for stopping parking.");
        return stopResult!;
    }

}