using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;
using WebApplication.Models.Validation.Authentication;

namespace WebApplication.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender<ApplicationUser> _emailSender;

    [BindProperty]
    public RegistrationModel RegistrationModel { get; set; }

    public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender<ApplicationUser> emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        if (await _userManager.FindByEmailAsync(RegistrationModel.EmailModel.Email ?? string.Empty) != null)
            ModelState.AddModelError("RegistrationModel.EmailModel.Email", $"Account with email '{RegistrationModel.EmailModel.Email}' already exists.");

        if (await _userManager.FindByNameAsync(RegistrationModel.UserNameModel.UserName ?? string.Empty) != null)
            ModelState.AddModelError("RegistrationModel.UserNameModel.UserName", $"Username '{RegistrationModel.UserNameModel.UserName}' is already taken.");

        if (!ModelState.IsValid)
            return Page();

        var user = new ApplicationUser(RegistrationModel.UserNameModel.UserName!) { Email = RegistrationModel.EmailModel.Email! };
        var result = await _userManager.CreateAsync(user, RegistrationModel.PasswordConfirmationModel.Password!);

        if (!result.Succeeded) 
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
        else
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string confirmationLink = Url.PageLink("ConfirmEmail", values: new { UserId = user.Id, Token = token });

            await _emailSender.SendConfirmationLinkAsync(user, RegistrationModel.EmailModel.Email!, confirmationLink);
            await _signInManager.PasswordSignInAsync(user, RegistrationModel.PasswordConfirmationModel.Password!, false, false);

            return RedirectToPage("RegistrationSucceeded");
        }
    }
}
