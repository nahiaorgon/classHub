using ClassHub.Web.Models;

namespace ClassHub.Web.Services;

public interface IForumService
{
    Task<List<ForumThread>> GetThreadsByUnitAsync(int unitId);
    Task<ForumThread?> GetThreadWithPostsAsync(int threadId);
    Task<ForumThread> CreateThreadAsync(int unitId, string title, int authorId);
    Task<ForumPost> AddPostAsync(int threadId, string content, int authorId, int? parentPostId = null);
    Task PinThreadAsync(int threadId, bool pinned);
}
