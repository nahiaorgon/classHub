namespace ClassHub.Web.Models;

public class Unit
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Resource> Resources { get; set; } = [];
    public ICollection<ForumThread> Threads { get; set; } = [];
}
