using ClassHub.Web.Models;

namespace ClassHub.Web.Services;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<Book> AddAsync(string title, string author, string description, string fileName, string filePath, int userId);
    Task DeleteAsync(int bookId);
}
