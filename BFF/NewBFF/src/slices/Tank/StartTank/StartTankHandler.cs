using Shared.SharedModels.TankModels;

namespace BFF.NewBFF.Slices.Tank.StartTank;

public class StartTankHandler
{
    private readonly ILogger<StartTankHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _tankServiceUrl;
    private readonly IConfiguration _configuration;

    public StartTankHandler(ILogger<StartTankHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _tankServiceUrl = configuration["ServiceUrls:TankService"] ?? string.Empty;
        _configuration = configuration;
    }

    /// <summary>
    /// Starts a tanking session and returns the result.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TankStatusResponse> HandleStartTankAsync(StartTankRequest request)
    {
        _logger.LogInformation("Calling TankService to start tank.");
        var response = await _httpClient.PostAsJsonAsync($"{_tankServiceUrl}/api/StartTank/StartTank", request);
        response.EnsureSuccessStatusCode();
        var tankResult = await response.Content.ReadFromJsonAsync<TankStatusResponse>();
        _logger.LogInformation("Received response from TankService for starting tank.");
        return tankResult!;
    }
}