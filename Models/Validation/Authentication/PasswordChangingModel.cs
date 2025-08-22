using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Validation.Authentication;

public class PasswordChangingModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = $"Password must be bigger than 8 and less than 30 chars.")]
    public string? CurrentPassword { get; set; }

    public PasswordConfirmationModel PasswordConfirmationModel { get; set; } = new();
}
