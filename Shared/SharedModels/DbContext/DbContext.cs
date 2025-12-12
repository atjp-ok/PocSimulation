using Microsoft.EntityFrameworkCore;

namespace Shared.SharedModels.SharedDbContext;
public class SharedDbContext : DbContext
{
    /// <summary>
    /// Constructor for SharedDbContext to store shared models.
    /// </summary>
    /// <param name="options"></param>
    public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options)
    {
    }
    // User Profiles
    public DbSet<UserModels.UserProfileModel> UserProfiles { get; set; }

    // PSp Models
    public DbSet<PSpModels.Request> Requests { get; set; }
    public DbSet<PSpModels.Response> Responses { get; set; }

    // Hammaq Models
    public DbSet<HammaqModels.HammaqRequest> HammaqRequests { get; set; }
    public DbSet<HammaqModels.HammaqResponse> HammaqResponses { get; set; }

    // Vask Models
    public DbSet<VaskModels.VaskRequest> VaskStatuses { get; set; }
    public DbSet<VaskModels.VaskStatusResponse> VaskStatusResponses { get; set; }
    public DbSet<VaskModels.VaskCompleteRequest> VaskCompleteRequests { get; set; }
    public DbSet<VaskModels.VaskCompleteResponse> VaskCompleteResponses { get; set; }

    // Tank Models
    public DbSet<TankModels.StartTankRequest> TankStatuses { get; set; }
    public DbSet<TankModels.TankStatusResponse> TankResponses { get; set; }
    public DbSet<TankModels.TankCompleteRequest> TankCompleteRequests { get; set; }
    public DbSet<TankModels.TankCompleteResponse> TankCompleteResponses { get; set; }

    // Appservice Models - Parking
    public DbSet<AppserviceModels.ParkingRequest> ParkingStatuses { get; set; }
    public DbSet<AppserviceModels.ParkingResponse> ParkingStatusResponses { get; set; }
    public DbSet<AppserviceModels.StopParking> StopParkingStatuses { get; set; }
    public DbSet<AppserviceModels.StopParkingResponse> StopParkingStatus { get; set; }
}
