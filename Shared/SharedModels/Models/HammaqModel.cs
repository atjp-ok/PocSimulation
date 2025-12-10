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
    public int ServiceId { get; set; } //id for the specific tank or vask session
    public string Status { get; set; } = "Pending"; //Pending, InProgress, Completed
    public string ServiceType { get; set; } = string.Empty; //tank or vask
}
