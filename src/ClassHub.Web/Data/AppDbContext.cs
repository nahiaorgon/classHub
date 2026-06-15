using ClassHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassHub.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<ForumThread> ForumThreads => Set<ForumThread>();
    public DbSet<ForumPost> ForumPosts => Set<ForumPost>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasConversion<string>();
        });

        modelBuilder.Entity<Resource>(e =>
        {
            e.Property(r => r.Type).HasConversion<string>();
            e.HasOne(r => r.Unit).WithMany(u => u.Resources).HasForeignKey(r => r.UnitId);
            e.HasOne(r => r.UploadedBy).WithMany(u => u.Resources).HasForeignKey(r => r.UploadedById);
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.HasOne(b => b.UploadedBy).WithMany(u => u.Books).HasForeignKey(b => b.UploadedById);
        });

        modelBuilder.Entity<ForumThread>(e =>
        {
            e.HasOne(t => t.Unit).WithMany(u => u.Threads).HasForeignKey(t => t.UnitId);
            e.HasOne(t => t.Author).WithMany(u => u.Threads).HasForeignKey(t => t.AuthorId);
        });

        modelBuilder.Entity<ForumPost>(e =>
        {
            e.HasOne(p => p.Thread).WithMany(t => t.Posts).HasForeignKey(p => p.ThreadId);
            e.HasOne(p => p.Author).WithMany(u => u.Posts).HasForeignKey(p => p.AuthorId);
            e.HasOne(p => p.ParentPost).WithMany(p => p.Replies).HasForeignKey(p => p.ParentPostId);
        });
    }
}
