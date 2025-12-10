using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Shared.SharedModels.VaskModels;
public class VaskRequest
{
    [DefaultValue(1)]
    [Key]
    public int VaskId { get; set; } //id for the specific vask session

    [DefaultValue(1)]
    public int UserId { get; set; }

    [DefaultValue(1)]
    public int StationId { get; set; }

    [DefaultValue("Basis")]
    public string Vasktype { get; set; } = "Basis";
}

public class VaskStatusResponse
{
    [Key]
    public int VaskId { get; set; } //id for the specific vask session
    public int UserId { get; set; } // User who requested the vask
    public int StationId { get; set; } // which station from hammaq
    public string CurrentStatus { get; set; } = "InProgress"; //pending,inprogress,completed
    public string Vasktype { get; set; } = "Basis"; //type of vask, Basis or Premium
    public double AmountToPay { get; set; }
    public string Message { get; set; } = string.Empty;

}

public class VaskCompleteRequest
{
    [DefaultValue(1)]
    [Key]
    public int VaskId { get; set; } //id for the specific vask session

    [DefaultValue(1)]
    public int StationId { get; set; } // which station from hammaq

    [DefaultValue(1)]
    public int UserId { get; set; } // User who requested the vask
}

public class VaskCompleteResponse
{
    [Key]
    public int VaskId { get; set; } //id for the specific vask session
    public int UserId { get; set; } // User who requested the vask
    public int StationId { get; set; } // which station from hammaq
    public string CurrentStatus { get; set; } = string.Empty; //pending,inprogress,completed
    public string Vasktype { get; set; } = string.Empty; //type of vask, Basis or Premium
    public double AmountToPay { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Category {get; set;} ="Vask";
}
