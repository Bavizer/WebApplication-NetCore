using WebApplication.Models.Validation.User;

namespace WebApplication.Models.Validation.Authentication;

public class LoginModel
{
    public UserNameModel UserNameModel { get; set; } = new();
    public PasswordModel PasswordModel { get; set; } = new();
}
