using Shared.SharedModels.SharedDbContext;
using Shared.SharedModels.HammaqModels;
using HammaqService.slices.Hammaq.CompleteHammaq;

namespace HammaqService.slices.StartHammaq;

public class StartHammaqHandler
{
    private readonly ILogger<StartHammaqHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly IServiceScopeFactory _serviceFactory;
    private readonly SharedDbContext _dbContext;
    private readonly CompleteHammaqHandler _completeHammaqHandler;
    public StartHammaqHandler(ILogger<StartHammaqHandler> logger, IConfiguration configuration, CompleteHammaqHandler completeHammaqHandler, HttpClient httpClient, SharedDbContext dbContext, IServiceScopeFactory serviceFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _completeHammaqHandler = completeHammaqHandler;
        _httpClient = httpClient;
        _dbContext = dbContext;
        _serviceFactory = serviceFactory;
    }

    public async Task<HammaqResponse> HandleStart(HammaqRequest request)
    {
        var status = new HammaqResponse
        {
            ServiceId = request.ServiceId,
            ServiceType = request.ServiceType,
            Status = "InProgress"
        };
        _dbContext.HammaqResponses.Add(status);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"Hammaq service started for {request.ServiceType} with ID {request.ServiceId} on station {request.StationId}.");
        
        // Background task to simulate completion after delay
        _ = Task.Run(async () =>
  {
      try
      {
          await Task.Delay(TimeSpan.FromSeconds(20)); // simulerer proces

          using var scope = _serviceFactory.CreateScope();
          var db = scope.ServiceProvider.GetRequiredService<SharedDbContext>();

          var statusToUpdate = db.HammaqResponses
              .FirstOrDefault(s => s.ServiceId == request.ServiceId && s.ServiceType == request.ServiceType);

          if (statusToUpdate != null)
          {
              statusToUpdate.Status = "Completed";
              db.Update(statusToUpdate);
              await db.SaveChangesAsync();
              _logger.LogInformation($"Hammaq service completed for {request.ServiceType} ID {request.ServiceId}");

              var completeHandler = scope.ServiceProvider.GetRequiredService<CompleteHammaqHandler>();
              await completeHandler.HandleComplete(new HammaqRequest
              {
                  ServiceId = request.ServiceId,
                  ServiceType = request.ServiceType,
                  StationId = request.StationId
              });
          }
      }
      catch (Exception ex)
      {
          _logger.LogError(ex, $"Error in Hammaq background task for {request.ServiceType} ID {request.ServiceId}");
      }
  });

        return status;
    }

}