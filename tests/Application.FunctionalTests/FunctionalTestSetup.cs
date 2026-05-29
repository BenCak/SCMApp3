using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SCMApp3.Data;

namespace SCMApp3.Application.FunctionalTests;

[SetUpFixture]
public class FunctionalTestSetup
{
    internal static IServiceScopeFactory ScopeFactory { get; private set; } = null!;
    internal static DatabaseResetter? DbResetter { get; private set; }

    private static WebApiFactory? _factory;
    private static string? _dbPath;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"scmapp3-test-{Guid.NewGuid()}.db");
        var connectionString = $"DataSource={_dbPath};Cache=Shared";

        _factory = new WebApiFactory(connectionString);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SCMDbContext>();
        await db.Database.EnsureCreatedAsync();

        ScopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        DbResetter = await DatabaseResetter.CreateAsync(connectionString);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (DbResetter is not null) await DbResetter.DisposeAsync();
        if (_factory is not null) await _factory.DisposeAsync();
        if (_dbPath is not null && File.Exists(_dbPath)) File.Delete(_dbPath);
    }
}
