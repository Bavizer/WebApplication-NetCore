using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;
using WebApplication.Models.Validation.Authentication;

namespace WebApplication.Pages.Auth;

public class SetNewPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    [BindProperty]
    public PasswordConfirmationModel PasswordConfirmationModel { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? UserId {  get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Token { get; set; }

    public SetNewPasswordModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet()
    {
        if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            return RedirectToPage("/Main/Home");

        var user = await _userManager.FindByIdAsync(UserId);

        if (user == null)
            return RedirectToPage("/Main/Home");

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _userManager.FindByIdAsync(UserId);

        if (user == null)
            return RedirectToPage();

        var result = await _userManager.ResetPasswordAsync(user, Token, PasswordConfirmationModel.Password);
        
        if (result.Succeeded)
            return RedirectToPage("PasswordResetSucceeded");

        return Page();
    }
}
