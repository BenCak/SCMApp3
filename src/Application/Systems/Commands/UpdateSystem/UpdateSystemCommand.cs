namespace SCMApp3.Application.Systems.Commands.UpdateSystem;

public record UpdateSystemCommand(
    int Id,
    int CustomerId,
    int ProductId,
    string? PocName,
    string? PocEmail,
    string? PocPhone) : IRequest;

public class UpdateSystemCommandValidator : AbstractValidator<UpdateSystemCommand>
{
    public UpdateSystemCommandValidator()
    {
        RuleFor(v => v.CustomerId).GreaterThan(0);
        RuleFor(v => v.ProductId).GreaterThan(0);
        RuleFor(v => v.PocEmail).EmailAddress().When(v => !string.IsNullOrWhiteSpace(v.PocEmail));
    }
}

public class UpdateSystemCommandHandler : IRequestHandler<UpdateSystemCommand>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public UpdateSystemCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task Handle(UpdateSystemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.Systems.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(ScmSystem), request.Id);

        entity.CustomerId = request.CustomerId;
        entity.ProductId = request.ProductId;
        entity.PocName = request.PocName;
        entity.PocEmail = request.PocEmail;
        entity.PocPhone = request.PocPhone;

        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Update", "System", entity.Id, _user.UserName, cancellationToken);
    }
}
