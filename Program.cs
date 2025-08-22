using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication;

public class Program
{
    public static void Main()
    {
        var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();

        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        { 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            options.UseLazyLoadingProxies();
        });

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Main/Home?handler=Logout";
        });

        builder.Services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapFallbackToPage("/Main/Home");

        app.Run();
    }
}
