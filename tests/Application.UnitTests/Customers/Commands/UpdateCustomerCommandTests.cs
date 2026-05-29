using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SCMApp3.Application.Common.Exceptions;
using SCMApp3.Application.Common.Interfaces;
using SCMApp3.Application.Customers.Commands.UpdateCustomer;
using SCMApp3.Application.UnitTests.Common;
using SCMApp3.Data;
using Shouldly;

namespace SCMApp3.Application.UnitTests.Customers.Commands;

[TestFixture]
public class UpdateCustomerCommandTests
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
        var validator = new UpdateCustomerCommandValidator();
        var result = validator.TestValidate(new UpdateCustomerCommand(1, "", null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_NameExceeds200Chars_HasError()
    {
        var validator = new UpdateCustomerCommandValidator();
        var result = validator.TestValidate(new UpdateCustomerCommand(1, new string('A', 201), null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validator_AbbreviationExceeds20Chars_HasError()
    {
        var validator = new UpdateCustomerCommandValidator();
        var result = validator.TestValidate(new UpdateCustomerCommand(1, "Acme", new string('X', 21)));
        result.ShouldHaveValidationErrorFor(x => x.Abbreviation);
    }

    [Test]
    public void Validator_ValidInput_NoErrors()
    {
        var validator = new UpdateCustomerCommandValidator();
        var result = validator.TestValidate(new UpdateCustomerCommand(1, "Acme Corp", "ACME"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    // --- Handler ---

    [Test]
    public async Task Handle_ExistingCustomer_UpdatesNameAndAbbreviation()
    {
        var customer = new Customer { Name = "Old Name", Abbreviation = "OLD" };
        _testDb.Db.Customers.Add(customer);
        await _testDb.Db.SaveChangesAsync();

        var handler = new UpdateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);
        await handler.Handle(new UpdateCustomerCommand(customer.Id, "New Name", "NEW"), CancellationToken.None);

        var updated = await _testDb.Db.Customers.FindAsync(customer.Id);
        updated!.Name.ShouldBe("New Name");
        updated.Abbreviation.ShouldBe("NEW");
    }

    [Test]
    public async Task Handle_ExistingCustomer_ClearsAbbreviationWhenNull()
    {
        var customer = new Customer { Name = "Acme", Abbreviation = "ACME" };
        _testDb.Db.Customers.Add(customer);
        await _testDb.Db.SaveChangesAsync();

        var handler = new UpdateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);
        await handler.Handle(new UpdateCustomerCommand(customer.Id, "Acme", null), CancellationToken.None);

        var updated = await _testDb.Db.Customers.FindAsync(customer.Id);
        updated!.Abbreviation.ShouldBeNull();
    }

    [Test]
    public async Task Handle_NonExistentId_ThrowsNotFoundException()
    {
        var handler = new UpdateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);

        await Should.ThrowAsync<NotFoundException>(() =>
            handler.Handle(new UpdateCustomerCommand(999, "Name", null), CancellationToken.None));
    }

    [Test]
    public async Task Handle_ExistingCustomer_LogsAuditEntry()
    {
        var customer = new Customer { Name = "Acme" };
        _testDb.Db.Customers.Add(customer);
        await _testDb.Db.SaveChangesAsync();

        var handler = new UpdateCustomerCommandHandler(_testDb.Db, _audit.Object, _user.Object);
        await handler.Handle(new UpdateCustomerCommand(customer.Id, "New Name", null), CancellationToken.None);

        _audit.Verify(a => a.LogAsync("Update", "Customer", customer.Id, "DOMAIN\\testuser", It.IsAny<CancellationToken>()), Times.Once);
    }
}
