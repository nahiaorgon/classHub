namespace ClassHub.Web.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Alumno;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Resource> Resources { get; set; } = [];
    public ICollection<Book> Books { get; set; } = [];
    public ICollection<ForumThread> Threads { get; set; } = [];
    public ICollection<ForumPost> Posts { get; set; } = [];
}

public enum UserRole
{
    Alumno,
    Profesor
}
