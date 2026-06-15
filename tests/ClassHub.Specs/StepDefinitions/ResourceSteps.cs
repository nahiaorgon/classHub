using ClassHub.Web.Data;
using ClassHub.Web.Models;
using ClassHub.Web.Services;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Xunit;

namespace ClassHub.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Gestión de recursos por unidad")]
public class ResourceSteps
{
    private readonly AppDbContext _db;
    private readonly ResourceService _resourceService;
    private List<Resource>? _queryResult;

    public ResourceSteps()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new AppDbContext(options);
        _resourceService = new ResourceService(_db);
    }

    [Given(@"existe la unidad ""(.*)"" con id (.*)")]
    public void GivenExisteUnidad(string title, int id)
    {
        if (!_db.Units.Any(u => u.Id == id))
        {
            _db.Units.Add(new Unit { Id = id, Number = id, Title = title, Description = title });
            _db.SaveChanges();
        }
    }

    [Given(@"existe el profesor ""(.*)"" con id (.*)")]
    public void GivenExisteProfesor(string name, int id)
    {
        if (!_db.Users.Any(u => u.Id == id))
        {
            _db.Users.Add(new AppUser { Id = id, Name = name, Email = $"{name.ToLower()}@test.com", PasswordHash = "x", Role = UserRole.Profesor });
            _db.SaveChanges();
        }
    }

    [Given(@"la unidad (.*) tiene el link ""(.*)"" con título ""(.*)""")]
    public async Task GivenUnidadTieneLink(int unitId, string url, string title)
    {
        const int seedUserId = 99;
        if (!_db.Users.Any(u => u.Id == seedUserId))
        {
            _db.Users.Add(new AppUser { Id = seedUserId, Name = "Seed", Email = "seed@test.com", PasswordHash = "x" });
            await _db.SaveChangesAsync();
        }
        await _resourceService.AddLinkAsync(unitId, title, "", url, seedUserId);
    }

    [When(@"el profesor con id (.*) agrega el link ""(.*)"" con título ""(.*)"" a la unidad (.*)")]
    public async Task WhenProfesorAgregaLink(int userId, string url, string title, int unitId)
        => await _resourceService.AddLinkAsync(unitId, title, "", url, userId);

    [When(@"el profesor con id (.*) agrega el archivo ""(.*)"" con título ""(.*)"" a la unidad (.*)")]
    public async Task WhenProfesorAgregaArchivo(int userId, string fileName, string title, int unitId)
        => await _resourceService.AddFileAsync(unitId, title, "", fileName, $"/tmp/{fileName}", userId);

    [When(@"se consultan los recursos de la unidad (.*)")]
    public async Task WhenSeConsultanRecursos(int unitId)
        => _queryResult = await _resourceService.GetByUnitAsync(unitId);

    [Then(@"la unidad (.*) tiene (.*) recurso de tipo Link")]
    public async Task ThenUnidadTieneLink(int unitId, int count)
    {
        var resources = await _resourceService.GetByUnitAsync(unitId);
        Assert.Equal(count, resources.Count(r => r.Type == ResourceType.Link));
    }

    [Then(@"la unidad (.*) tiene (.*) recurso de tipo Pdf")]
    public async Task ThenUnidadTienePdf(int unitId, int count)
    {
        var resources = await _resourceService.GetByUnitAsync(unitId);
        Assert.Equal(count, resources.Count(r => r.Type == ResourceType.Pdf));
    }

    [Then(@"el resultado está vacío")]
    public void ThenResultadoVacio()
    {
        Assert.NotNull(_queryResult);
        Assert.Empty(_queryResult);
    }
}
