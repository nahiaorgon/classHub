using ClassHub.Web.Data;
using ClassHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassHub.Web.Services;

public class ResourceService(AppDbContext db) : IResourceService
{
    public Task<List<Resource>> GetByUnitAsync(int unitId)
        => db.Resources
            .Include(r => r.UploadedBy)
            .Where(r => r.UnitId == unitId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<Resource> AddLinkAsync(int unitId, string title, string description, string url, int userId)
    {
        var resource = new Resource
        {
            UnitId = unitId,
            Title = title,
            Description = description,
            Type = ResourceType.Link,
            Url = url,
            UploadedById = userId
        };
        db.Resources.Add(resource);
        await db.SaveChangesAsync();
        return resource;
    }

    public async Task<Resource> AddFileAsync(int unitId, string title, string description, string fileName, string filePath, int userId)
    {
        var resource = new Resource
        {
            UnitId = unitId,
            Title = title,
            Description = description,
            Type = ResourceType.Pdf,
            FileName = fileName,
            FilePath = filePath,
            UploadedById = userId
        };
        db.Resources.Add(resource);
        await db.SaveChangesAsync();
        return resource;
    }

    public async Task DeleteAsync(int resourceId)
    {
        var resource = await db.Resources.FindAsync(resourceId);
        if (resource is not null)
        {
            if (resource.FilePath is not null && File.Exists(resource.FilePath))
                File.Delete(resource.FilePath);
            db.Resources.Remove(resource);
            await db.SaveChangesAsync();
        }
    }
}
