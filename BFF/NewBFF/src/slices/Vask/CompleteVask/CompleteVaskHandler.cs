using Shared.SharedModels.VaskModels;

namespace BFF.NewBFF.Slices.Vask.CompleteVask;

public class CompleteVaskHandler
{
    private readonly ILogger<CompleteVaskHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _vaskServiceUrl;
    private readonly IConfiguration _configuration;

    public CompleteVaskHandler(ILogger<CompleteVaskHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _vaskServiceUrl = configuration["ServiceUrls:VaskService"] ?? string.Empty;
        _configuration = configuration;
    }

    /// <summary>
    /// Completes a vask session and returns the result.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<VaskCompleteResponse> HandleCompleteVaskAsync(VaskCompleteRequest request)
    {
        _logger.LogInformation("Calling VaskService to complete vask.");
        var response = await _httpClient.PostAsJsonAsync($"{_vaskServiceUrl}/api/CompleteVask/CompleteVask", request);
        response.EnsureSuccessStatusCode();
        var vaskCompleteResult = await response.Content.ReadFromJsonAsync<VaskCompleteResponse>();
        _logger.LogInformation("Received response from VaskService for completing vask.");
        return vaskCompleteResult!;
    }

}