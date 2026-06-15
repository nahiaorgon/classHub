using ClassHub.Web.Data;
using ClassHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassHub.Web.Services;

public class UnitService(AppDbContext db) : IUnitService
{
    public Task<List<Unit>> GetAllAsync()
        => db.Units.OrderBy(u => u.Number).ToListAsync();

    public Task<Unit?> GetByIdAsync(int id)
        => db.Units
            .Include(u => u.Resources)
            .Include(u => u.Threads).ThenInclude(t => t.Author)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<Unit> CreateAsync(string title, string description, int number)
    {
        var unit = new Unit { Title = title, Description = description, Number = number };
        db.Units.Add(unit);
        await db.SaveChangesAsync();
        return unit;
    }
}
