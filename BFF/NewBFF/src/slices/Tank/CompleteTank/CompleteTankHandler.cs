using Shared.SharedModels.TankModels;

namespace BFF.NewBFF.Slices.Tank.CompleteTank;
public class CompleteTankHandler
{
    private readonly ILogger<CompleteTankHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _tankServiceUrl;
    private readonly IConfiguration _configuration;

    public CompleteTankHandler(ILogger<CompleteTankHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _tankServiceUrl = configuration["ServiceUrls:TankService"] ?? string.Empty;
        _configuration = configuration;
    }
    public async Task<TankCompleteResponse> HandleTankCompleteAsync(TankCompleteRequest request)
    {
        _logger.LogInformation("Calling TankService to complete tank.");
        var response = await _httpClient.PostAsJsonAsync($"{_tankServiceUrl}/api/CompleteTank/CompleteTank", request);
        response.EnsureSuccessStatusCode();
        var tankCompleteResult = await response.Content.ReadFromJsonAsync<TankCompleteResponse>();
        _logger.LogInformation("Received response from TankService for completing tank.");
        return tankCompleteResult!;
    }

}