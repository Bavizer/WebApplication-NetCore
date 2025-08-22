using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Models;

[Authorize]
public class AuthUserPageModel : PageModel
{
    protected readonly UserManager<ApplicationUser> userManager;
    public ApplicationUser ApplicationUser { get; set; }

    public AuthUserPageModel(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        ApplicationUser = await userManager.GetUserAsync(User);

        await next();
    }
}
