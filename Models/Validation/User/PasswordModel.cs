using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Validation.User;

public class PasswordModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = $"Password must be bigger than 8 and less than 30 chars.")]
    public string? Password { get; set; }
}
