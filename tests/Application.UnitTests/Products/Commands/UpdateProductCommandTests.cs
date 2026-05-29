using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SCMApp3.Application.Common.Exceptions;
using SCMApp3.Application.Common.Interfaces;
using SCMApp3.Application.Products.Commands.UpdateProduct;
using SCMApp3.Application.UnitTests.Common;
using SCMApp3.Data;
using Shouldly;

namespace SCMApp3.Application.UnitTests.Products.Commands;

[TestFixture]
public class UpdateProductCommandTests
{
    private TestDbContext _testDb = null!;
    private Mock<IAuditService> _audit = null!;
    private Mock<IUser> _user = null!;

    [SetUp]
    public void SetUp()
    {
        _testDb = new TestDbContext();
        _audit = new Mock<IAuditService>();
        _user = new Mock<IUser>();
        _user.SetupGet(u => u.UserName).Returns("DOMAIN\\testuser");
    }

    [TearDown]
    public void TearDown() => _testDb.Dispose();

    // --- Validator ---

    [Test]
    public void Validator_EmptyName_HasError()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(1, "", null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_NameExceeds200Chars_HasError()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(1, new string('A', 201), null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_ValidInput_NoErrors()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(1, "My Product", "Updated description"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    // --- Handler ---

    [Test]
    public async Task Handle_ExistingProduct_UpdatesNameAndDescription()
    {
        var product = new Product { Name = "Old Product", Description = "Old description" };
        _testDb.Db.Products.Add(product);
        await _testDb.Db.SaveChangesAsync();

        var handler = new UpdateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);
        await handler.Handle(new UpdateProductCommand(product.Id, "New Product", "New description"), CancellationToken.None);

        var updated = await _testDb.Db.Products.FindAsync(product.Id);
        updated!.Name.ShouldBe("New Product");
        updated.Description.ShouldBe("New description");
    }

    [Test]
    public async Task Handle_ExistingProduct_ClearsDescriptionWhenNull()
    {
        var product = new Product { Name = "My Product", Description = "Has a description" };
        _testDb.Db.Products.Add(product);
        await _testDb.Db.SaveChangesAsync();

        var handler = new UpdateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);
        await handler.Handle(new UpdateProductCommand(product.Id, "My Product", null), CancellationToken.None);

        var updated = await _testDb.Db.Products.FindAsync(product.Id);
        updated!.Description.ShouldBeNull();
    }

    [Test]
    public async Task Handle_NonExistentId_ThrowsNotFoundException()
    {
        var handler = new UpdateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        await Should.ThrowAsync<NotFoundException>(() =>
            handler.Handle(new UpdateProductCommand(999, "Name", null), CancellationToken.None));
    }

    [Test]
    public async Task Handle_ExistingProduct_LogsAuditEntry()
    {
        var product = new Product { Name = "My Product" };
        _testDb.Db.Products.Add(product);
        await _testDb.Db.SaveChangesAsync();

        var handler = new UpdateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);
        await handler.Handle(new UpdateProductCommand(product.Id, "Updated", null), CancellationToken.None);

        _audit.Verify(a => a.LogAsync("Update", "Product", product.Id, "DOMAIN\\testuser", It.IsAny<CancellationToken>()), Times.Once);
    }
}
