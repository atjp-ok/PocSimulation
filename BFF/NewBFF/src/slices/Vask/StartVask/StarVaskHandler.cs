using Shared.SharedModels.VaskModels;

namespace BFF.NewBFF.Slices.Vask.StartVask;

public class StartVaskHandler
{
    private readonly ILogger<StartVaskHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _vaskServiceUrl;
    private readonly IConfiguration _configuration;

    public StartVaskHandler(ILogger<StartVaskHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _vaskServiceUrl = configuration["ServiceUrls:VaskService"] ?? string.Empty;
        _configuration = configuration;
    }

    /// <summary>
    /// Starts a vask session and returns the result.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<VaskStatusResponse> HandleStartVaskAsync(VaskRequest request)
    {
        _logger.LogInformation("Calling VaskService to start vask.");
        var response = await _httpClient.PostAsJsonAsync($"{_vaskServiceUrl}/api/StartVask/StartVask", request);
        response.EnsureSuccessStatusCode();
        var vaskResult = await response.Content.ReadFromJsonAsync<VaskStatusResponse>();
        _logger.LogInformation("Received response from VaskService for starting vask.");
        return vaskResult!;
    }

}