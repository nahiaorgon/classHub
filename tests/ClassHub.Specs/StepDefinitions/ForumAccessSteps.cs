using ClassHub.Web.Data;
using ClassHub.Web.Models;
using ClassHub.Web.Services;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Xunit;

namespace ClassHub.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Control de acceso al foro por rol")]
public class ForumAccessSteps
{
    private readonly AppDbContext _db;
    private readonly ForumService _forumService;
    private ForumThread? _lastCreatedThread;
    private readonly Dictionary<string, ForumThread> _threads = new();

    public ForumAccessSteps()
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
        if (!_db.Units.Any(u => u.Id == id))
        {
            _db.Units.Add(new Unit { Id = id, Number = id, Title = title, Description = title });
            _db.SaveChanges();
        }
    }

    [Given(@"existe el alumno ""(.*)"" con id (.*)")]
    public void GivenExisteAlumno(string name, int id)
    {
        if (!_db.Users.Any(u => u.Id == id))
        {
            _db.Users.Add(new AppUser { Id = id, Name = name, Email = $"{name.ToLower()}@test.com", PasswordHash = "x", Role = UserRole.Alumno });
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

    [Given(@"existe el hilo ""(.*)"" en la unidad (.*) creado por el usuario (.*)")]
    public async Task GivenExisteHilo(string title, int unitId, int authorId)
    {
        var thread = await _forumService.CreateThreadAsync(unitId, title, authorId);
        _threads[title] = thread;
        _lastCreatedThread = thread;
    }

    [When(@"el alumno con id (.*) crea un hilo con título ""(.*)"" en la unidad (.*)")]
    public async Task WhenAlumnoCreaHilo(int authorId, string title, int unitId)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            var thread = await _forumService.CreateThreadAsync(unitId, title, authorId);
            _threads[title] = thread;
            _lastCreatedThread = thread;
        }
    }

    [When(@"el profesor con id (.*) fija el hilo")]
    public async Task WhenProfesorFijaUltimo(int _)
    {
        Assert.NotNull(_lastCreatedThread);
        await _forumService.PinThreadAsync(_lastCreatedThread.Id, pinned: true);
    }

    [When(@"el profesor con id (.*) fija el hilo ""(.*)"" en la unidad (.*)")]
    public async Task WhenProfesorFijaPorTitulo(int _, string title, int unitId)
    {
        Assert.True(_threads.ContainsKey(title), $"Hilo '{title}' no fue creado en el Background.");
        await _forumService.PinThreadAsync(_threads[title].Id, pinned: true);
    }

    [When(@"el alumno con id (.*) responde ""(.*)"" en el hilo ""(.*)""")]
    public async Task WhenAlumnoRespondeEnHilo(int authorId, string content, string threadTitle)
    {
        Assert.True(_threads.ContainsKey(threadTitle), $"Hilo '{threadTitle}' no existe.");
        await _forumService.AddPostAsync(_threads[threadTitle].Id, content, authorId);
    }

    [Then(@"el hilo ""(.*)"" está fijado")]
    public async Task ThenHiloEstaFijado(string title)
    {
        Assert.True(_threads.ContainsKey(title));
        var thread = await _forumService.GetThreadWithPostsAsync(_threads[title].Id);
        Assert.NotNull(thread);
        Assert.True(thread.IsPinned);
    }

    [Then(@"el primer hilo de la unidad (.*) es ""(.*)""")]
    public async Task ThenPrimerHiloEs(int unitId, string title)
    {
        var threads = await _forumService.GetThreadsByUnitAsync(unitId);
        Assert.Equal(title, threads.First().Title);
    }

    [Then(@"el hilo ""(.*)"" existe en la unidad (.*)")]
    public async Task ThenHiloExiste(string title, int unitId)
    {
        var threads = await _forumService.GetThreadsByUnitAsync(unitId);
        Assert.Contains(threads, t => t.Title == title);
    }

    [Then(@"el hilo ""(.*)"" no está fijado")]
    public async Task ThenHiloNoEstaFijado(string title)
    {
        Assert.True(_threads.ContainsKey(title));
        var thread = await _forumService.GetThreadWithPostsAsync(_threads[title].Id);
        Assert.NotNull(thread);
        Assert.False(thread.IsPinned);
    }

    [Then(@"el hilo ""(.*)"" tiene al menos (.*) respuesta")]
    public async Task ThenHiloTieneAlMenos(string title, int minCount)
    {
        Assert.True(_threads.ContainsKey(title));
        var thread = await _forumService.GetThreadWithPostsAsync(_threads[title].Id);
        Assert.NotNull(thread);
        Assert.True(thread.Posts.Count >= minCount);
    }
}
