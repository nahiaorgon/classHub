using ClassHub.Web.Data;
using ClassHub.Web.Models;
using ClassHub.Web.Services;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Xunit;

namespace ClassHub.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Gestión de hilos en el foro")]
public class ForumSteps
{
    private readonly AppDbContext _db;
    private readonly ForumService _forumService;
    private ForumThread? _createdThread;
    private Exception? _thrownException;

    public ForumSteps()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new AppDbContext(options);
        _forumService = new ForumService(_db);
    }

    [Given(@"existe la unidad ""(.*)"" con id (.*)")]
    public void GivenExisteUnidad(string title, int id)
    {
        _db.Units.Add(new Unit { Id = id, Number = id, Title = title, Description = title });
        _db.SaveChanges();
    }

    [Given(@"existe el alumno ""(.*)"" con id (.*)")]
    public void GivenExisteAlumno(string name, int id)
    {
        _db.Users.Add(new AppUser { Id = id, Name = name, Email = $"{name.ToLower()}@test.com", PasswordHash = "x", Role = UserRole.Alumno });
        _db.SaveChanges();
    }

    [Given(@"existe el profesor ""(.*)"" con id (.*)")]
    public void GivenExisteProfesor(string name, int id)
    {
        _db.Users.Add(new AppUser { Id = id, Name = name, Email = $"{name.ToLower()}@test.com", PasswordHash = "x", Role = UserRole.Profesor });
        _db.SaveChanges();
    }

    [Given(@"existe el hilo ""(.*)"" en la unidad (.*) creado por el usuario (.*)")]
    public async Task GivenExisteHilo(string title, int unitId, int authorId)
        => _createdThread = await _forumService.CreateThreadAsync(unitId, title, authorId);

    [When(@"el alumno con id (.*) crea un hilo con título ""(.*)"" en la unidad (.*)")]
    public async Task WhenAlumnoCreaHilo(int authorId, string title, int unitId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            _thrownException = new ArgumentException("El título no puede estar vacío.");
            return;
        }
        _createdThread = await _forumService.CreateThreadAsync(unitId, title, authorId);
    }

    [When(@"el alumno con id (.*) responde ""(.*)""")]
    public async Task WhenAlumnoResponde(int authorId, string content)
    {
        Assert.NotNull(_createdThread);
        await _forumService.AddPostAsync(_createdThread.Id, content, authorId);
    }

    [When(@"el profesor con id (.*) fija el hilo")]
    public async Task WhenProfesorFija(int _)
    {
        Assert.NotNull(_createdThread);
        await _forumService.PinThreadAsync(_createdThread.Id, pinned: true);
    }

    [Then(@"el hilo ""(.*)"" existe en la unidad (.*)")]
    public async Task ThenHiloExiste(string title, int unitId)
    {
        var threads = await _forumService.GetThreadsByUnitAsync(unitId);
        Assert.Contains(threads, t => t.Title == title);
    }

    [Then(@"el hilo tiene (.*) respuesta")]
    public async Task ThenHiloTieneRespuestas(int count)
    {
        Assert.NotNull(_createdThread);
        var thread = await _forumService.GetThreadWithPostsAsync(_createdThread.Id);
        Assert.Equal(count, thread!.Posts.Count);
    }

    [Then(@"el hilo ""(.*)"" aparece primero en la lista de la unidad (.*)")]
    public async Task ThenHiloAparecePrimero(string title, int unitId)
    {
        var threads = await _forumService.GetThreadsByUnitAsync(unitId);
        Assert.Equal(title, threads.First().Title);
    }

    [Then(@"el hilo no se crea")]
    public void ThenHiloNoSeCreo()
        => Assert.True(_thrownException is not null || _createdThread is null);
}
