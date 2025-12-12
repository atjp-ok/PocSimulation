using System.ComponentModel.DataAnnotations;

namespace Shared.SharedModels.HammaqModels;
public class HammaqRequest
{
    [Key]
    public int Id { get; set; }
    public string ServiceType { get; set; } = string.Empty; //Tank, or vask 
    public int ServiceId { get; set; } // tankId or vaskId
    public int StationId { get; set; } // stationId where the service is requested
    public double ActualLiters { get; set; }

}
public class HammaqResponse
{
    [Key]
    public int Id { get; set; }
    public int ServiceId { get; set; } 
    public string Status { get; set; } = string.Empty; // InProgress or Completed
    public string ServiceType { get; set; } = string.Empty;
}
