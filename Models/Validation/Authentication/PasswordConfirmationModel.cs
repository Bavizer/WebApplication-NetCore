using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Validation.Authentication;

public class PasswordConfirmationModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = $"Password must be bigger than 8 and less than 30 chars.")]
    public string? Password { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password confirmation is required!")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = $"Password must be bigger than 8 and less than 30 chars.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords must be equal.")]
    public string? PasswordConfirmation { get; set; }
}
