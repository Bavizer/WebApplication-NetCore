using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;

namespace WebApplication.Pages.Main;

[Authorize]
public class HomeModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnGetLogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage();
    }
}
