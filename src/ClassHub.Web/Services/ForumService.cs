using ClassHub.Web.Data;
using ClassHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassHub.Web.Services;

public class ForumService(AppDbContext db) : IForumService
{
    public Task<List<ForumThread>> GetThreadsByUnitAsync(int unitId)
        => db.ForumThreads
            .Include(t => t.Author)
            .Include(t => t.Posts)
            .Where(t => t.UnitId == unitId)
            .OrderByDescending(t => t.IsPinned)
            .ThenByDescending(t => t.CreatedAt)
            .ToListAsync();

    public Task<ForumThread?> GetThreadWithPostsAsync(int threadId)
        => db.ForumThreads
            .Include(t => t.Author)
            .Include(t => t.Posts.OrderBy(p => p.CreatedAt))
                .ThenInclude(p => p.Author)
            .FirstOrDefaultAsync(t => t.Id == threadId);

    public async Task<ForumThread> CreateThreadAsync(int unitId, string title, int authorId)
    {
        var thread = new ForumThread
        {
            UnitId = unitId,
            Title = title,
            AuthorId = authorId
        };
        db.ForumThreads.Add(thread);
        await db.SaveChangesAsync();
        return thread;
    }

    public async Task<ForumPost> AddPostAsync(int threadId, string content, int authorId, int? parentPostId = null)
    {
        var post = new ForumPost
        {
            ThreadId = threadId,
            Content = content,
            AuthorId = authorId,
            ParentPostId = parentPostId
        };
        db.ForumPosts.Add(post);
        await db.SaveChangesAsync();
        return post;
    }

    public async Task PinThreadAsync(int threadId, bool pinned)
    {
        var thread = await db.ForumThreads.FindAsync(threadId);
        if (thread is not null)
        {
            thread.IsPinned = pinned;
            await db.SaveChangesAsync();
        }
    }
}
