using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Pages.Auth;

public class ConfirmEmailModel : AuthUserPageModel
{
    public ConfirmEmailModel(UserManager<ApplicationUser> userManager) : base(userManager) { }

    public async Task<IActionResult> OnGet(string? userId, string? token)
    {
        if (ApplicationUser.EmailConfirmed)
            return RedirectToPage("/Main/Home");

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            ViewData["Result"] = "The link is invalid or expired";
            return Page();
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            ViewData["Result"] = "User not found";
            return Page();
        }
            
        var result = await userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded) 
        {
            ViewData["Result"] = "Email is successfully confirmed!";
            return Page();
        }

        ViewData["Result"] = "An error occured while confirming your email";
        return Page();
    }
}
