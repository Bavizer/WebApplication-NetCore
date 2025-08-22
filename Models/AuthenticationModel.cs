using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models;

public class AuthenticationModel
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(18, MinimumLength = 4, ErrorMessage = $"{nameof(Login)} must be bigger than 4 and less than 18 characters.")]
    public string? Login { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(18, MinimumLength = 8, ErrorMessage = $"{nameof(Password)} must be bigger than 8 and less than 18 characters.")]
    public string? Password { get; set; }
}
