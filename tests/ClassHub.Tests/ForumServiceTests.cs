using ClassHub.Web.Data;
using ClassHub.Web.Models;
using ClassHub.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClassHub.Tests;

public class ForumServiceTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly ForumService _sut;

    public ForumServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new AppDbContext(options);
        _sut = new ForumService(_db);

        SeedData();
    }

    private void SeedData()
    {
        var user = new AppUser { Id = 1, Name = "Alumno Test", Email = "a@test.com", PasswordHash = "x" };
        var unit = new Unit { Id = 1, Number = 1, Title = "Unidad 1", Description = "Desc" };
        _db.Users.Add(user);
        _db.Units.Add(unit);
        _db.SaveChanges();
    }

    [Fact]
    public async Task CreateThreadAsync_ValidData_ReturnsThread()
    {
        // Act
        var thread = await _sut.CreateThreadAsync(unitId: 1, title: "Mi primer hilo", authorId: 1);

        // Assert
        Assert.NotNull(thread);
        Assert.Equal("Mi primer hilo", thread.Title);
        Assert.Equal(1, thread.UnitId);
    }

    [Fact]
    public async Task GetThreadsByUnitAsync_PinnedFirst_PinnedThreadAppearsFirst()
    {
        // Arrange
        var t1 = await _sut.CreateThreadAsync(1, "Hilo normal", 1);
        var t2 = await _sut.CreateThreadAsync(1, "Hilo fijado", 1);
        await _sut.PinThreadAsync(t2.Id, pinned: true);

        // Act
        var threads = await _sut.GetThreadsByUnitAsync(1);

        // Assert
        Assert.Equal(t2.Id, threads.First().Id);
        Assert.True(threads.First().IsPinned);
    }

    [Fact]
    public async Task AddPostAsync_ValidData_PostLinkedToThread()
    {
        // Arrange
        var thread = await _sut.CreateThreadAsync(1, "Hilo con posts", 1);

        // Act
        var post = await _sut.AddPostAsync(thread.Id, "Primera respuesta", authorId: 1);

        // Assert
        Assert.Equal(thread.Id, post.ThreadId);
        Assert.Equal("Primera respuesta", post.Content);
    }

    [Fact]
    public async Task AddPostAsync_WithParent_IsReply()
    {
        // Arrange
        var thread = await _sut.CreateThreadAsync(1, "Hilo", 1);
        var parent = await _sut.AddPostAsync(thread.Id, "Post padre", 1);

        // Act
        var reply = await _sut.AddPostAsync(thread.Id, "Respuesta", 1, parentPostId: parent.Id);

        // Assert
        Assert.Equal(parent.Id, reply.ParentPostId);
    }

    [Fact]
    public async Task PinThreadAsync_Pin_IsPinnedTrue()
    {
        // Arrange
        var thread = await _sut.CreateThreadAsync(1, "Hilo", 1);
        Assert.False(thread.IsPinned);

        // Act
        await _sut.PinThreadAsync(thread.Id, pinned: true);
        var updated = await _sut.GetThreadWithPostsAsync(thread.Id);

        // Assert
        Assert.True(updated!.IsPinned);
    }

    [Fact]
    public async Task GetThreadWithPostsAsync_NonExistentId_ReturnsNull()
    {
        var result = await _sut.GetThreadWithPostsAsync(99999);
        Assert.Null(result);
    }

    public void Dispose() => _db.Dispose();
}
