using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.SharedModels.AppserviceModels;
public class ParkingRequest
{
    [DefaultValue(1)]
    [Key]
    public int ParkingId { get; set; }

    [DefaultValue(1)]
    public int UserId { get; set; }

    [DefaultValue("Aarhus C")]
    public string? Location { get; set; }
}
public class ParkingResponse
{
    [Key]
    public int ParkingId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartTime { get; set; }
    public int UserId { get; set; }
}

public class StopParking
{
    [DefaultValue(1)]
    [Key]
    public int ParkingId { get; set; }
}
public class StopParkingResponse
{
    [Key]
    public int ParkingId { get; set; }
    public string? Status { get; set; } = "Completed";
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public double? AmountToPay { get; set; }
    public string? Message { get; set; }
     public string Category {get; set;} ="Parking";
}