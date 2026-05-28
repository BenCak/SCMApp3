namespace SCMApp3.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(string Name, string? Abbreviation) : IRequest<int>;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
        RuleFor(v => v.Abbreviation).MaximumLength(20);
    }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public CreateCustomerCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer { Name = request.Name, Abbreviation = request.Abbreviation };
        _db.Customers.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Create", "Customer", entity.Id, _user.UserName, cancellationToken);
        return entity.Id;
    }
}
