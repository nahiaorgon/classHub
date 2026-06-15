using ClassHub.Web.Models;

namespace ClassHub.Web.Services;

public interface IUnitService
{
    Task<List<Unit>> GetAllAsync();
    Task<Unit?> GetByIdAsync(int id);
    Task<Unit> CreateAsync(string title, string description, int number);
}
