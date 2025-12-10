
using System.ComponentModel.DataAnnotations;

namespace Shared.SharedModels.UserModels;
public class UserProfileModel
{
    [Key]
    public int UserId { get; set; } //id Identifier for the user
    public string Name { get; set; } = string.Empty; //Name of the user
}
