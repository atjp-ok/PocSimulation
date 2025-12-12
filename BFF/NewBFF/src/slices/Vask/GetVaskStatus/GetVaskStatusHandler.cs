using Shared.SharedModels.VaskModels;

namespace BFF.NewBFF.Slices.Vask.GetVaskStatus;

public class GetStatusVaskHandler
{
    private readonly ILogger<GetStatusVaskHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _vaskServiceUrl;
    private readonly IConfiguration _configuration;

    public GetStatusVaskHandler(ILogger<GetStatusVaskHandler> logger, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;
        _vaskServiceUrl = configuration["ServiceUrls:VaskService"] ?? string.Empty;
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the status of a vask session for a given vask ID and returns the result.
    /// </summary>
    /// <param name="vaskId"></param>
    /// <returns></returns>
    public async Task<VaskStatusResponse> GetVaskStatus(int vaskId)
    {
        _logger.LogInformation("Calling VaskService to get vask status.");
        var response = await _httpClient.GetAsync($"{_vaskServiceUrl}/api/GetStatusVask/GetStatusVask?vaskId={vaskId}");
        response.EnsureSuccessStatusCode();
        var vaskStatusResult = await response.Content.ReadFromJsonAsync<VaskStatusResponse>();
        _logger.LogInformation("Received response from VaskService for getting vask status.");
        return vaskStatusResult!;

    }

}