using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SCMApp3.Application.Common.Interfaces;
using SCMApp3.Application.Products.Commands.CreateProduct;
using SCMApp3.Application.UnitTests.Common;
using Shouldly;

namespace SCMApp3.Application.UnitTests.Products.Commands;

[TestFixture]
public class CreateProductCommandTests
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
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand("", null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_NameExceeds200Chars_HasError()
    {
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(new string('A', 201), null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_ValidInput_NoErrors()
    {
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand("My Product", "A description"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_NullDescription_NoErrors()
    {
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand("My Product", null));
        result.ShouldNotHaveAnyValidationErrors();
    }

    // --- Handler ---

    [Test]
    public async Task Handle_ValidCommand_ReturnsPersistentId()
    {
        var handler = new CreateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateProductCommand("My Product", "A description"), CancellationToken.None);

        id.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task Handle_ValidCommand_PersistsProductWithCorrectValues()
    {
        var handler = new CreateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateProductCommand("My Product", "A description"), CancellationToken.None);

        var saved = await _testDb.Db.Products.FindAsync(id);
        saved.ShouldNotBeNull();
        saved!.Name.ShouldBe("My Product");
        saved.Description.ShouldBe("A description");
    }

    [Test]
    public async Task Handle_NullDescription_PersistsNullDescription()
    {
        var handler = new CreateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateProductCommand("My Product", null), CancellationToken.None);

        var saved = await _testDb.Db.Products.FindAsync(id);
        saved!.Description.ShouldBeNull();
    }

    [Test]
    public async Task Handle_ValidCommand_LogsAuditEntry()
    {
        var handler = new CreateProductCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateProductCommand("My Product", null), CancellationToken.None);

        _audit.Verify(a => a.LogAsync("Create", "Product", id, "DOMAIN\\testuser", It.IsAny<CancellationToken>()), Times.Once);
    }
}
