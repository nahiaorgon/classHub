namespace ClassHub.Web.Models;

public class ForumPost
{
    public int Id { get; set; }
    public int ThreadId { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? ParentPostId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ForumThread Thread { get; set; } = null!;
    public AppUser Author { get; set; } = null!;
    public ForumPost? ParentPost { get; set; }
    public ICollection<ForumPost> Replies { get; set; } = [];
}
