namespace WebApplication.Models;

public class ProfilePost
{
    public string Id { get; set; }

    public virtual ApplicationUser Author { get; set; }
    public virtual ApplicationUser Profile { get; set; }

    public string? Content { get; set; }
    public string? MediaPath { get; set; }

    public DateTime CreationDateTime { get; private set; } = DateTime.Now;

    public ProfilePost() { }

    public ProfilePost(ApplicationUser author, string content, string? mediaPath = null)
    {
        Id = Guid.NewGuid().ToString();
        Author = author;
        Content = content;
        MediaPath = mediaPath;
    }
}
