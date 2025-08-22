using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ProfilePost> ProfilePosts { get; private set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
