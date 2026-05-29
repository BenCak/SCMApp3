using NUnit.Framework;
using SCMApp3.Application.Customers.Queries.GetCustomers;
using SCMApp3.Application.UnitTests.Common;
using SCMApp3.Data;
using Shouldly;

namespace SCMApp3.Application.UnitTests.Customers.Queries;

[TestFixture]
public class GetCustomersQueryTests
{
    private TestDbContext _testDb = null!;

    [SetUp]
    public void SetUp() => _testDb = new TestDbContext();

    [TearDown]
    public void TearDown() => _testDb.Dispose();

    [Test]
    public async Task Handle_NoCustomers_ReturnsEmptyList()
    {
        var handler = new GetCustomersQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

        result.ShouldBeEmpty();
    }

    [Test]
    public async Task Handle_WithCustomers_ReturnsAllCustomers()
    {
        _testDb.Db.Customers.AddRange(
            new Customer { Name = "Beta Corp" },
            new Customer { Name = "Alpha Inc" });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetCustomersQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

        result.Count.ShouldBe(2);
    }

    [Test]
    public async Task Handle_WithCustomers_ReturnsOrderedByName()
    {
        _testDb.Db.Customers.AddRange(
            new Customer { Name = "Zeta Ltd" },
            new Customer { Name = "Alpha Inc" },
            new Customer { Name = "Beta Corp" });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetCustomersQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

        result.Select(c => c.Name).ShouldBe(["Alpha Inc", "Beta Corp", "Zeta Ltd"]);
    }

    [Test]
    public async Task Handle_WithCustomers_MapsDtoFieldsCorrectly()
    {
        _testDb.Db.Customers.Add(new Customer { Name = "Acme Corp", Abbreviation = "ACME" });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetCustomersQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

        var dto = result.Single();
        dto.Id.ShouldBeGreaterThan(0);
        dto.Name.ShouldBe("Acme Corp");
        dto.Abbreviation.ShouldBe("ACME");
    }

    [Test]
    public async Task Handle_CustomerWithNullAbbreviation_MapsNullAbbreviation()
    {
        _testDb.Db.Customers.Add(new Customer { Name = "Acme Corp", Abbreviation = null });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetCustomersQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

        result.Single().Abbreviation.ShouldBeNull();
    }
}
