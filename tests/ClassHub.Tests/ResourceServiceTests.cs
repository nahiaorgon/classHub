using ClassHub.Web.Data;
using ClassHub.Web.Models;
using ClassHub.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClassHub.Tests;

public class ResourceServiceTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly ResourceService _sut;

    public ResourceServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new AppDbContext(options);
        _sut = new ResourceService(_db);

        SeedData();
    }

    private void SeedData()
    {
        _db.Users.Add(new AppUser { Id = 1, Name = "Profesor", Email = "p@test.com", PasswordHash = "x", Role = UserRole.Profesor });
        _db.Units.Add(new Unit { Id = 1, Number = 1, Title = "Unidad 1", Description = "Desc" });
        _db.SaveChanges();
    }

    [Fact]
    public async Task AddLinkAsync_ValidData_ResourceCreated()
    {
        // Act
        var resource = await _sut.AddLinkAsync(
            unitId: 1,
            title: "Video de IC",
            description: "Explicación de CI",
            url: "https://youtube.com/watch?v=abc",
            userId: 1);

        // Assert
        Assert.NotNull(resource);
        Assert.Equal(ResourceType.Link, resource.Type);
        Assert.Equal("https://youtube.com/watch?v=abc", resource.Url);
    }

    [Fact]
    public async Task GetByUnitAsync_ReturnsOnlyResourcesForUnit()
    {
        // Arrange
        await _sut.AddLinkAsync(1, "Link unidad 1", "", "https://u1.com", 1);
        _db.Units.Add(new Unit { Id = 2, Number = 2, Title = "Unidad 2", Description = "Desc" });
        await _db.SaveChangesAsync();
        await _sut.AddLinkAsync(2, "Link unidad 2", "", "https://u2.com", 1);

        // Act
        var results = await _sut.GetByUnitAsync(1);

        // Assert
        Assert.Single(results);
        Assert.All(results, r => Assert.Equal(1, r.UnitId));
    }

    [Fact]
    public async Task AddFileAsync_ValidData_PdfResourceCreated()
    {
        // Act
        var resource = await _sut.AddFileAsync(
            unitId: 1,
            title: "Clase 1",
            description: "Diapositivas",
            fileName: "clase1.pdf",
            filePath: "/tmp/clase1.pdf",
            userId: 1);

        // Assert
        Assert.Equal(ResourceType.Pdf, resource.Type);
        Assert.Equal("clase1.pdf", resource.FileName);
    }

    [Fact]
    public async Task GetByUnitAsync_OrderByCreatedAtDesc_NewestFirst()
    {
        // Arrange
        await _sut.AddLinkAsync(1, "Primero", "", "https://a.com", 1);
        await _sut.AddLinkAsync(1, "Segundo", "", "https://b.com", 1);

        // Act
        var results = await _sut.GetByUnitAsync(1);

        // Assert — newest is first
        Assert.Equal("Segundo", results.First().Title);
    }

    public void Dispose() => _db.Dispose();
}
