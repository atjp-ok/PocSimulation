using Shared.SharedModels.PSpModels;
using Shared.SharedModels.SharedDbContext;

namespace PspService.slices.Reserve;
public class ReserveHandler
{
    private readonly ILogger<ReserveHandler> _logger;
    private readonly SharedDbContext _dbContext;
    public ReserveHandler(ILogger<ReserveHandler> logger, SharedDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Reserves a payment for a service and stores the reservation.
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    public async Task<Response> HandleReserve(Request payment)
    {
        _logger.LogInformation($"Reserving Payment | User: {payment.UserId}, Service:{payment.ServiceType} ({payment.ServiceId}), Amount: {payment.Amount}");
        var status = new Response
        {
            ServiceId = payment.ServiceId,
            ServiceType = payment.ServiceType,
            Status = "Reserved"
        };
        _dbContext.Requests.Add(payment);
        _dbContext.Responses.Add(status);
       await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"Payment reserved for {payment.ServiceType} with ID {payment.ServiceId}.");
        return await Task.FromResult(status);
    }
}