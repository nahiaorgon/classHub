namespace ClassHub.Web.Models;

public class Resource
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ResourceType Type { get; set; }

    // For PDF files
    public string? FileName { get; set; }
    public string? FilePath { get; set; }

    // For links
    public string? Url { get; set; }

    public int UploadedById { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Unit Unit { get; set; } = null!;
    public AppUser UploadedBy { get; set; } = null!;
}

public enum ResourceType
{
    Pdf,
    Link
}
