using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SCMApp3.Data;

namespace SCMApp3.Application.UnitTests.Common;

public sealed class TestDbContext : IDisposable
{
    private readonly SqliteConnection _connection;
    public SCMDbContext Db { get; }

    public TestDbContext()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<SCMDbContext>()
            .UseSqlite(_connection)
            .Options;

        Db = new SCMDbContext(options);
        Db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Db.Dispose();
        _connection.Dispose();
    }
}
