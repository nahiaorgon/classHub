using ClassHub.Web.Data;
using ClassHub.Web.Models;
using ClassHub.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClassHub.Tests;

public class AuthServiceTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new AppDbContext(options);
        _sut = new AuthService(_db);

        SeedData();
    }

    private void SeedData()
    {
        _db.Users.Add(new AppUser
        {
            Id = 1,
            Name = "Nahia",
            Email = "nahia@classhub.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("segura123"),
            Role = UserRole.Profesor
        });
        _db.SaveChanges();
    }

    [Fact]
    public async Task ValidateAsync_CorrectCredentials_ReturnsUser()
    {
        // Act
        var user = await _sut.ValidateAsync("nahia@classhub.com", "segura123");

        // Assert
        Assert.NotNull(user);
        Assert.Equal("Nahia", user.Name);
    }

    [Fact]
    public async Task ValidateAsync_WrongPassword_ReturnsNull()
    {
        // Act
        var user = await _sut.ValidateAsync("nahia@classhub.com", "incorrecta");

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task ValidateAsync_UnknownEmail_ReturnsNull()
    {
        // Act
        var user = await _sut.ValidateAsync("noexiste@classhub.com", "cualquier");

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task ValidateAsync_EmptyPassword_ReturnsNull()
    {
        var user = await _sut.ValidateAsync("nahia@classhub.com", "");
        Assert.Null(user);
    }

    public void Dispose() => _db.Dispose();
}
