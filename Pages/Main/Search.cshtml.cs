using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;

namespace WebApplication.Pages.Main;

public class SearchModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    [BindProperty(SupportsGet = true)]
    public string? UserName { get; set; }

    public ApplicationUser? SearchResult { get; set; }

    public SearchModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet()
    {
        SearchResult = string.IsNullOrEmpty(UserName) ? null : await _userManager.FindByNameAsync(UserName);
        return Page();
    }
}
