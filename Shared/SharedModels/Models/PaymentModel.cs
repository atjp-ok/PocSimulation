using System.ComponentModel.DataAnnotations;

namespace Shared.SharedModels.PSpModels;
public class Request
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; } //id of the user making the payment
    public string ServiceType { get; set; } = string.Empty; //Tank, or vask 
    public int ServiceId { get; set; } // tankId or vaskId
    public double Amount { get; set; } // amount to be reserved or captured
}


public class Response
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }    // <-- Tilføj denne linje!
    public double Amount { get; set; } // <-- Og denne!
    public int ServiceId { get; set; } //id for the specific tank or vask session
    public string Status { get; set; } = string.Empty; //Reserved, Captured, Failed
    public string ServiceType { get; set; } = string.Empty; //tank or vask
}