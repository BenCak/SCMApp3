namespace SCMApp3.Application.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(int Id, string Name, string? Abbreviation) : IRequest;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.Abbreviation).MaximumLength(20);
    }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public UpdateCustomerCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.Customers.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.Id);

        entity.Name = request.Name;
        entity.Abbreviation = request.Abbreviation;

        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Update", "Customer", entity.Id, _user.UserName, cancellationToken);
    }
}
