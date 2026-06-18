using ClassHub.Web.Data;
using ClassHub.Web.Models;
using ClassHub.Web.Services;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Xunit;

namespace ClassHub.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Autenticación de usuarios")]
public class AuthenticationSteps
{
    private readonly AppDbContext _db;
    private readonly AuthService _authService;
    private AppUser? _validationResult;

    public AuthenticationSteps()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new AppDbContext(options);
        _authService = new AuthService(_db);
    }

    [Given(@"existe el alumno ""(.*)"" con email ""(.*)"" y contraseña ""(.*)""")]
    public void GivenExisteAlumno(string name, string email, string password)
    {
        _db.Users.Add(new AppUser
        {
            Name         = name,
            Email        = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role         = UserRole.Alumno
        });
        _db.SaveChanges();
    }

    [Given(@"existe el profesor ""(.*)"" con email ""(.*)"" y contraseña ""(.*)""")]
    public void GivenExisteProfesor(string name, string email, string password)
    {
        _db.Users.Add(new AppUser
        {
            Name         = name,
            Email        = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role         = UserRole.Profesor
        });
        _db.SaveChanges();
    }

    [When(@"se validan las credenciales ""(.*)"" y ""(.*)""")]
    public async Task WhenSeValidan(string email, string password)
        => _validationResult = await _authService.ValidateAsync(email, password);

    [Then(@"el usuario retornado es ""(.*)"" con rol Alumno")]
    public void ThenUsuarioAlumno(string name)
    {
        Assert.NotNull(_validationResult);
        Assert.Equal(name, _validationResult.Name);
        Assert.Equal(UserRole.Alumno, _validationResult.Role);
    }

    [Then(@"el usuario retornado es ""(.*)"" con rol Profesor")]
    public void ThenUsuarioProfesor(string name)
    {
        Assert.NotNull(_validationResult);
        Assert.Equal(name, _validationResult.Name);
        Assert.Equal(UserRole.Profesor, _validationResult.Role);
    }

    [Then(@"el resultado de validación es nulo")]
    public void ThenResultadoNulo()
        => Assert.Null(_validationResult);

    [Then(@"el usuario retornado NO tiene el rol Profesor")]
    public void ThenNoEsProfesor()
    {
        Assert.NotNull(_validationResult);
        Assert.NotEqual(UserRole.Profesor, _validationResult.Role);
    }
}
