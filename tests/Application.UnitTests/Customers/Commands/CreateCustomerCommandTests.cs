using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SCMApp3.Application.Common.Exceptions;
using SCMApp3.Application.Common.Interfaces;
using SCMApp3.Application.Customers.Commands.CreateCustomer;
using SCMApp3.Application.UnitTests.Common;
using Shouldly;

namespace SCMApp3.Application.UnitTests.Customers.Commands;

[TestFixture]
public class CreateCustomerCommandTests
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
        var validator = new CreateCustomerCommandValidator();
        var result = validator.TestValidate(new CreateCustomerCommand("", null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_NameExceeds200Chars_HasError()
    {
        var validator = new CreateCustomerCommandValidator();
        var result = validator.TestValidate(new CreateCustomerCommand(new string('A', 201), null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_AbbreviationExceeds20Chars_HasError()
    {
        var validator = new CreateCustomerCommandValidator();
        var result = validator.TestValidate(new CreateCustomerCommand("Acme", new string('X', 21)));
        result.ShouldHaveValidationErrorFor(x => x.Abbreviation);
    }

    [Test]
    public void Validator_ValidInput_NoErrors()
    {
        var validator = new CreateCustomerCommandValidator();
        var result = validator.TestValidate(new CreateCustomerCommand("Acme Corp", "ACME"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_NullAbbreviation_NoErrors()
    {
        var validator = new CreateCustomerCommandValidator();
        var result = validator.TestValidate(new CreateCustomerCommand("Acme Corp", null));
        result.ShouldNotHaveAnyValidationErrors();
    }

    // --- Handler ---

    [Test]
    public async Task Handle_ValidCommand_ReturnsPersistentId()
    {
        var handler = new CreateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateCustomerCommand("Acme Corp", "ACME"), CancellationToken.None);

        id.ShouldBeGreaterThan(0);
    }

    [Test]
    public async Task Handle_ValidCommand_PersistsCustomerWithCorrectValues()
    {
        var handler = new CreateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateCustomerCommand("Acme Corp", "ACME"), CancellationToken.None);

        var saved = await _testDb.Db.Customers.FindAsync(id);
        saved.ShouldNotBeNull();
        saved!.Name.ShouldBe("Acme Corp");
        saved.Abbreviation.ShouldBe("ACME");
    }

    [Test]
    public async Task Handle_NullAbbreviation_PersistsNullAbbreviation()
    {
        var handler = new CreateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateCustomerCommand("Acme Corp", null), CancellationToken.None);

        var saved = await _testDb.Db.Customers.FindAsync(id);
        saved!.Abbreviation.ShouldBeNull();
    }

    [Test]
    public async Task Handle_ValidCommand_LogsAuditEntry()
    {
        var handler = new CreateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        var id = await handler.Handle(new CreateCustomerCommand("Acme Corp", "ACME"), CancellationToken.None);

        _audit.Verify(a => a.LogAsync("Create", "Customer", id, "DOMAIN\\testuser", It.IsAny<CancellationToken>()), Times.Once);
    }
}
