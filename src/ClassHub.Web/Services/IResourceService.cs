using ClassHub.Web.Models;

namespace ClassHub.Web.Services;

public interface IResourceService
{
    Task<List<Resource>> GetByUnitAsync(int unitId);
    Task<Resource> AddLinkAsync(int unitId, string title, string description, string url, int userId);
    Task<Resource> AddFileAsync(int unitId, string title, string description, string fileName, string filePath, int userId);
    Task DeleteAsync(int resourceId);
}
