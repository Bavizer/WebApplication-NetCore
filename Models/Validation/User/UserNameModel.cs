using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Validation.User;

public class UserNameModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required!")]
    [StringLength(18, MinimumLength = 4, ErrorMessage = "Username must be bigger than 4 and less than 18 chars.")]
    public string? UserName { get; set; }
}