using NUnit.Framework;
using SCMApp3.Application.Products.Queries.GetProducts;
using SCMApp3.Application.UnitTests.Common;
using SCMApp3.Data;
using Shouldly;

namespace SCMApp3.Application.UnitTests.Products.Queries;

[TestFixture]
public class GetProductsQueryTests
{
    private TestDbContext _testDb = null!;

    [SetUp]
    public void SetUp() => _testDb = new TestDbContext();

    [TearDown]
    public void TearDown() => _testDb.Dispose();

    [Test]
    public async Task Handle_NoProducts_ReturnsEmptyList()
    {
        var handler = new GetProductsQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        result.ShouldBeEmpty();
    }

    [Test]
    public async Task Handle_WithProducts_ReturnsAllProducts()
    {
        _testDb.Db.Products.AddRange(
            new Product { Name = "Beta Suite" },
            new Product { Name = "Alpha Tool" });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        result.Count.ShouldBe(2);
    }

    [Test]
    public async Task Handle_WithProducts_ReturnsOrderedByName()
    {
        _testDb.Db.Products.AddRange(
            new Product { Name = "Zeta Platform" },
            new Product { Name = "Alpha Tool" },
            new Product { Name = "Beta Suite" });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        result.Select(p => p.Name).ShouldBe(["Alpha Tool", "Beta Suite", "Zeta Platform"]);
    }

    [Test]
    public async Task Handle_WithProducts_MapsDtoFieldsCorrectly()
    {
        _testDb.Db.Products.Add(new Product { Name = "My Product", Description = "A description" });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        var dto = result.Single();
        dto.Id.ShouldBeGreaterThan(0);
        dto.Name.ShouldBe("My Product");
        dto.Description.ShouldBe("A description");
    }

    [Test]
    public async Task Handle_ProductWithNullDescription_MapsNullDescription()
    {
        _testDb.Db.Products.Add(new Product { Name = "My Product", Description = null });
        await _testDb.Db.SaveChangesAsync();

        var handler = new GetProductsQueryHandler(_testDb.Db);

        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

        result.Single().Description.ShouldBeNull();
    }
}
