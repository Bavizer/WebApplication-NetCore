using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;

namespace WebApplication.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    [BindProperty]
    public Models.Validation.Authentication.LoginModel AuthLoginModel { get; set; }

    public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _signInManager.PasswordSignInAsync(AuthLoginModel.UserNameModel.UserName!, AuthLoginModel.PasswordModel.Password!, false, false);

        if (result.Succeeded)
            return RedirectToPage("/Main/Home");

        ModelState.AddModelError(string.Empty, "Wrong login or password");
        return Page();
    }
}
