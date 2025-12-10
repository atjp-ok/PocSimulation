
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.SharedModels.TankModels;
public class StartTankRequest
{
    [DefaultValue(1)]
    [Key]
    public int TankId { get; set; } //id for the specific tank session

    [DefaultValue(1)]
    public int StationId { get; set; } // which station from hammaq

    [DefaultValue("95")]
    public string Fueltype { get; set; } = "95"; //diesel, 95

    [DefaultValue(1)]
    public int UserId { get; set; } //User requesting the tank if they have an account
}

public class TankStatusResponse
{
    [Key]
    public int TankId { get; set; } //id for the specific tank session

    public string CurrentStatus { get; set; } = string.Empty; //pending,inprogress,completed
    public string Fueltype { get; set; } = string.Empty; //diesel, 95

    // [JsonPropertyName("amountLiters")]
    // public double ActualLiters { get; set; } // amount to fill, to use for payment reservation
    public int StationId { get; set; } // which station from hammaq
    public int UserId { get; set; } //User requesting the tank if they have an account
}


public class TankCompleteRequest
{
    [DefaultValue(1)]
    [Key]
    public int TankId { get; set; } //id for the specific tank session

    [DefaultValue(1)]
    public int StationId { get; set; } // which station from hammaq

    [DefaultValue(10.0)]
    public double ActualLiters { get; set; } // amount filled

    [DefaultValue(1)]
    public int UserId { get; set; } // User who requested the tank
}


public class TankCompleteResponse
{
    [Key]
    public int TankId { get; set; } //id for the specific tank session
    public string CurrentStatus { get; set; } = "Completed"; //Pending, InProgress, Completed
    public int UserId { get; set; } // User who requested the tank
    public double AmountLiters { get; set; } // amount filled
    public string Fueltype { get; set; } = string.Empty; //diesel, 95
    public double AmountCharged { get; set; }
    public int StationId { get; set; } // which station from hammaq
    public string Message { get; set; } = string.Empty;
     public string Category {get; set;} ="Tank";
}