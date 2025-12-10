using Shared.SharedModels.TankModels;

namespace BFF.NewBFF.Slices.Tank.GetTankStatus;
public class GetTankStatusHandler
{
    private readonly ILogger<GetTankStatusHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _tankServiceUrl;
    private readonly IConfiguration _configuration;

    public GetTankStatusHandler(ILogger<GetTankStatusHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _tankServiceUrl = configuration["ServiceUrls:TankService"] ?? string.Empty;
        _configuration = configuration;
    }

    public async Task<TankStatusResponse> HandleGetTankStatusAsync(int tankId)
    {
        _logger.LogInformation("Calling TankService to get tank status.");
        var response = await _httpClient.GetAsync($"{_tankServiceUrl}/api/GetTankStatus/GetTankStatus/{tankId}");
        response.EnsureSuccessStatusCode();
        var tankStatusResult = await response.Content.ReadFromJsonAsync<TankStatusResponse>();
        _logger.LogInformation("Received response from TankService for getting tank status.");
        return tankStatusResult!;
    }
}