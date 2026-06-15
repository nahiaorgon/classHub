namespace ClassHub.Web.Models;

public class ForumThread
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsPinned { get; set; } = false;
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Unit Unit { get; set; } = null!;
    public AppUser Author { get; set; } = null!;
    public ICollection<ForumPost> Posts { get; set; } = [];
}
