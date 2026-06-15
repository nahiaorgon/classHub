using ClassHub.Web.Data;
using ClassHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassHub.Web.Services;

public class BookService(AppDbContext db) : IBookService
{
    public Task<List<Book>> GetAllAsync()
        => db.Books
            .Include(b => b.UploadedBy)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<Book> AddAsync(string title, string author, string description, string fileName, string filePath, int userId)
    {
        var book = new Book
        {
            Title = title,
            Author = author,
            Description = description,
            FileName = fileName,
            FilePath = filePath,
            UploadedById = userId
        };
        db.Books.Add(book);
        await db.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(int bookId)
    {
        var book = await db.Books.FindAsync(bookId);
        if (book is not null)
        {
            if (book.FilePath is not null && File.Exists(book.FilePath))
                File.Delete(book.FilePath);
            db.Books.Remove(book);
            await db.SaveChangesAsync();
        }
    }
}
