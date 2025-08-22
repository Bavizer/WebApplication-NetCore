using WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Pages.Auth;

public class RegistrationSucceededModel : AuthUserPageModel
{
    public RegistrationSucceededModel(UserManager<ApplicationUser> userManager) : base(userManager) { }

    public IActionResult OnGet()
    {
        if (ApplicationUser.EmailConfirmed)
            return RedirectToPage("/Main/Home");

        return Page();
    }
}
