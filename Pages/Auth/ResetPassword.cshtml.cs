using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;
using WebApplication.Models.Validation.User;

namespace WebApplication.Pages.Auth;

public class ResetPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender<ApplicationUser> _emailSender;

    [BindProperty]
    public EmailModel EmailModel { get; set; }

    public ResetPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) 
            return Page();

        var user = await _userManager.FindByEmailAsync(EmailModel.Email!);

        if (user == null)
        {
            ModelState.AddModelError("Email", "User with this email doesn't exist.");
            return Page();
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string resetLink = Url.PageLink("SetNewPassword", values: new { UserId = user.Id, Token = token });

        await _emailSender.SendPasswordResetLinkAsync(user, EmailModel.Email, resetLink);
        return RedirectToPage("~/Auth/Login");
    }
}
