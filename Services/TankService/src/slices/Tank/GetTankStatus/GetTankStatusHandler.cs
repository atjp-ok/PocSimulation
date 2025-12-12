using Shared.SharedModels.HammaqModels;
using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.TankModels;

namespace TankService.slices.Tank.GetTankStatus
{
    public class GetTankStatusHandler
    {
        private readonly ILogger<GetTankStatusHandler> _logger;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly SharedDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _hammaqServiceUrl;

        public GetTankStatusHandler(ILogger<GetTankStatusHandler> logger, HttpClient httpClient, IConfiguration configuration, SharedDbContext dbContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _dbContext = dbContext;
            _hammaqServiceUrl = configuration["ServiceUrls:HammaqService"] ?? string.Empty;
        }

        /// <summary>
        /// Gets the current status of a tanking session.
        /// </summary>
        public async Task<TankStatusResponse> HandleTankStatus(int tankId)
        {
            _logger.LogInformation($"Retrieving status for TankId: {tankId}.");
            var tank = _dbContext.TankResponses.FirstOrDefault(t => t.TankId == tankId);
            if (tank == null)
            {
                _logger.LogInformation($"Tank not found for TankId: {tankId}.");
                throw new Exception("Tank not found");
            }

            var hammaqStatus = await _httpClient.GetFromJsonAsync<HammaqResponse>
            ($"{_hammaqServiceUrl}/api/GetStatus/GetStatus?ServiceType=Tank&ServiceId={tank.TankId}");
            if (hammaqStatus != null)
            {
                tank.CurrentStatus = hammaqStatus.Status;
                _dbContext.TankResponses.Update(tank);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Retrived Tank status for TankId: {tankId} is {hammaqStatus.Status}.");
            }
            return await Task.FromResult(tank);
        }
    }
}