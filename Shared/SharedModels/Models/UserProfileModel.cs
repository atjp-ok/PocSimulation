
using System.ComponentModel.DataAnnotations;

namespace Shared.SharedModels.UserModels;
public class UserProfileModel
{
    [Key]
    public int UserId { get; set; } 
    public string Name { get; set; } = string.Empty; 
}
