using Shared.SharedModels.PSpModels;
using Shared.SharedModels.SharedDbContext;

namespace PspService.slices.Capture;
public class CaptureHandler
{
    private readonly ILogger<CaptureHandler> _logger;
    private readonly SharedDbContext _dbContext;
    public CaptureHandler(ILogger<CaptureHandler> logger, SharedDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<Response> HandleCapture(Request payment)
    {
        _logger.LogInformation($"Capturing Payment | User: {payment.UserId}, Service:{payment.ServiceType} ({payment.ServiceId}), Amount: {payment.Amount}");
        var existingStatus = _dbContext.Responses
            .FirstOrDefault(r => r.ServiceId == payment.ServiceId && r.ServiceType == payment.ServiceType && r.Status == "Reserved");
        if (existingStatus != null)
        {
            existingStatus.Status = "Captured";
            existingStatus.Amount = payment.Amount;
            _dbContext.Responses.Update(existingStatus);
           await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Payment captured for {payment.ServiceType} with ID {payment.ServiceId}.");
            return existingStatus;
        }
        _logger.LogWarning($"Tried to capture unknown Payment {payment.ServiceType} ({payment.ServiceId})");
        return await Task.FromResult(new Response
        {
            ServiceId = payment.ServiceId,
            ServiceType = payment.ServiceType,
            Status = "NotFound"
        });
    }

}