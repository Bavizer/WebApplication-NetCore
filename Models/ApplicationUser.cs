using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models;

public class ApplicationUser : IdentityUser
{
    public string? AvatarPath { get; set; }
    public string? ProfileDescription { get; set; }
    public virtual List<ApplicationUser> Followers { get; private set; }
    public virtual List<ApplicationUser> Following { get; private set; }

    [InverseProperty("Author")]
    public virtual List<ProfilePost> AuthoredPosts { get; private set; } = [];

    [InverseProperty("Profile")]
    public virtual List<ProfilePost> ProfilePosts { get; private set; } = [];
    public DateTime RegistrationDateTime { get; private set; } = DateTime.Now;

    public ApplicationUser() : base() { }

    public ApplicationUser(string userName) : base(userName) { }
}
