using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;

namespace WebApplication.Pages.Profile;

public class ProfileModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public ApplicationUser? UserProfile { get; private set; }

    public ProfileModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> OnGet(string user)
    {
        if (string.IsNullOrEmpty(user))
            return BadRequest();

        UserProfile = await _userManager.FindByNameAsync(user.Replace("%20", " "));
        if (UserProfile == null)
            return BadRequest();

        return Page();
    }

    public async Task<IActionResult> OnPostFollowUser(string user)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        UserProfile = await _userManager.FindByNameAsync(user);

        if (UserProfile == null)
            return RedirectToPage("/Main/Home");

        if (currentUser == null)
            return Challenge();

        if (!UserProfile.Followers.Remove(currentUser))
            UserProfile.Followers.Add(currentUser);

        await _dbContext.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostProfilePostSubmiting(string user, string? content, IFormFile? file)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        UserProfile = await _userManager.FindByNameAsync(user);
        string? mediaPath = null;

        if (UserProfile == null)
            return RedirectToPage("/Main/Home");

        if (content == null && file == null)
        {
            ModelState.AddModelError(string.Empty, "You can not post nothing!");
            return Page();
        }

        if (content != null && content.Length > 1000)
        {
            ModelState.AddModelError(string.Empty, "Text must be less than 1000 chars.");
            return Page();
        }

        if (file != null)
        {
            string[] allowedExtensions = { ".png", ".jpg", ".gif" };
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                ModelState.AddModelError(string.Empty, $"Allowed file extensions: {string.Join(" | ", allowedExtensions)}");

            if (file.Length >= 5_000_000)
                ModelState.AddModelError(string.Empty, "File is larger than 5 MB!");

            if (!ModelState.IsValid)
                return Page();
            
            string fileName = string.Concat(currentUser!.Id, DateTime.Now.ToString("_dd.MM.yyyy-HH.mm.ss"), extension);
            string filePath = "images/posts/" + fileName;

            using (var stream = System.IO.File.Create(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath)))
            {
                await file.CopyToAsync(stream);
            }

            mediaPath = "/" + filePath;
        }

        var post = new ProfilePost(currentUser, content, mediaPath);
        UserProfile.ProfilePosts.Add(post);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteProfilePost(string id, string user)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        UserProfile = await _userManager.FindByNameAsync(user);

        if (UserProfile == null || UserProfile != currentUser)
            return RedirectToPage("/Main/Home");

        var post = UserProfile.ProfilePosts.SingleOrDefault(post => post.Id == id);

        if (post != null)
        {
            if (post.MediaPath != null)
            {
                try
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.MediaPath.TrimStart('\\', '/')));
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    return RedirectToPage();
                }
            }

            _dbContext.ProfilePosts.Remove(post);
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}
