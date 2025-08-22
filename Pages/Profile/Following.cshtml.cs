using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;

namespace WebApplication.Pages.Profile;

public class FollowingModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUser? UserProfile { get; private set; }
    public ApplicationUser? CurrentUser { get; private set; }

    public FollowingModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet(string userName)
    {
        if (string.IsNullOrEmpty(userName))
            return BadRequest();

        UserProfile = await _userManager.FindByNameAsync(userName.ToString());
        if (UserProfile == null)
            return BadRequest();

        CurrentUser = await _userManager.GetUserAsync(User);
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveFollowing(string userName, string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return RedirectToPage();

        UserProfile = await _userManager.FindByNameAsync(userName.ToString());
        if (UserProfile == null)
            return RedirectToPage("/Main/Home");

        UserProfile.Following.Remove(user);
        await _userManager.UpdateAsync(UserProfile);
        return RedirectToPage();
    }
}
