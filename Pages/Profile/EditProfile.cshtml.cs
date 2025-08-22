using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Models.Validation.Authentication;
using WebApplication.Models.Validation.User;

namespace WebApplication.Pages.Profile;

[ValidateAntiForgeryToken]
public class EditProfileModel : AuthUserPageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public EditProfileModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(userManager)
    {
        _signInManager = signInManager;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostUpdateProfilePicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("ProfilePicture", "File is not selected!");
            return Page();
        }

        string[] allowedExtensions = { ".png", ".jpg", ".gif" };
        string extension = Path.GetExtension(file.FileName).ToLower();

        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension)) 
            ModelState.AddModelError("ProfilePicture", $"Allowed file extensions: {string.Join(" | ", allowedExtensions)}");

        if (file.Length >= 2_000_000)
            ModelState.AddModelError("ProfilePicture", "File is larger than 2 MB!");

        if (!ModelState.IsValid)
            return Page();

        string fileName = string.Concat(ApplicationUser!.Id, DateTime.Now.ToString("_dd.MM.yyyy-HH.mm.ss"), extension);
        string filePath = "images/ProfilePictures/" + fileName;

        using (var stream = System.IO.File.Create(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath)))
        {
            await file.CopyToAsync(stream);
        }

        ApplicationUser!.AvatarPath = "/" + filePath;
        await userManager.UpdateAsync(ApplicationUser);

        return RedirectToPage("Profile", new { User = ApplicationUser.UserName });
    }

    public async Task<IActionResult> OnPostUpdateUserName(UserNameModel model)
    {
        if (!ModelState.IsValid)
            return Page();

        if (await userManager.FindByNameAsync(model.UserName!) != null)
        {
            ModelState.AddModelError("UserName", $"Username '{model.UserName!}' is already taken.");
            return Page();
        }

        ApplicationUser!.UserName = model.UserName!;
        await userManager.UpdateAsync(ApplicationUser);

        return RedirectToPage("Profile", new { user = ApplicationUser!.UserName! });
    }

    public async Task<IActionResult> OnPostUpdateProfileDescription(string? profileDescription)
    {
        if (!ModelState.ContainsKey(nameof(profileDescription)))
            return Page();

        if (profileDescription?.Length > 200)
        {
            ModelState.AddModelError(nameof(profileDescription), "Description must be less than 100 chars.");
            return Page();
        }

        ApplicationUser!.ProfileDescription = profileDescription ?? string.Empty;
        await userManager.UpdateAsync(ApplicationUser);

        return RedirectToPage("Profile", new { User = ApplicationUser.UserName });
    }

    public async Task<IActionResult> OnPostUpdatePassword(PasswordChangingModel model)
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await userManager.ChangePasswordAsync(ApplicationUser, model.CurrentPassword!, model.PasswordConfirmationModel.Password!);

        if (result.Succeeded)
            return RedirectToPage("Profile", new { user = ApplicationUser!.UserName });

        foreach (var error in result.Errors)
            ModelState.AddModelError("CurrentPassword", error.Description);

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteProfile()
    {
        await _signInManager.SignOutAsync();
        var result = await userManager.DeleteAsync(ApplicationUser);

        if (result.Succeeded)
            return RedirectToPage("/Auth/Login");

        foreach (var error in result.Errors)
            ModelState.AddModelError("UserName", error.Description);

        return Page();
    }
}
