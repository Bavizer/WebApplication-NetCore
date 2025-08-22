using WebApplication.Models.Validation.User;

namespace WebApplication.Models.Validation.Authentication;

public class RegistrationModel
{
    public EmailModel EmailModel { get; set; } = new();
    public UserNameModel UserNameModel { get; set; } = new();
    public PasswordConfirmationModel PasswordConfirmationModel { get; set; } = new();
}
