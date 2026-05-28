namespace SCMApp3.Application.Systems.Commands.CreateSystem;

public record CreateSystemCommand(
    int CustomerId,
    int ProductId,
    string? PocName,
    string? PocEmail,
    string? PocPhone,
    int? ParentId = null) : IRequest<int>;

public class CreateSystemCommandValidator : AbstractValidator<CreateSystemCommand>
{
    public CreateSystemCommandValidator()
    {
        RuleFor(v => v.CustomerId).GreaterThan(0);
        RuleFor(v => v.ProductId).GreaterThan(0);
        RuleFor(v => v.PocEmail).EmailAddress().When(v => !string.IsNullOrWhiteSpace(v.PocEmail));
    }
}

public class CreateSystemCommandHandler : IRequestHandler<CreateSystemCommand, int>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public CreateSystemCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task<int> Handle(CreateSystemCommand request, CancellationToken cancellationToken)
    {
        var entity = new ScmSystem
        {
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            ParentId = request.ParentId,
            PocName = request.PocName,
            PocEmail = request.PocEmail,
            PocPhone = request.PocPhone
        };

        _db.Systems.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Create", "System", entity.Id, _user.UserName, cancellationToken);
        return entity.Id;
    }
}
