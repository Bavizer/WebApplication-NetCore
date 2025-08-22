using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Validation.User;

public class EmailModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
}