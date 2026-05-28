namespace SCMApp3.Application.ReleaseRequests.Commands.CreateReleaseRequest;

public record CreateReleaseRequestCommand(
    string? Location,
    string? ReleaseDate,
    int? ReleaseTypeId,
    string? Notes) : IRequest<int>;

public class CreateReleaseRequestCommandValidator : AbstractValidator<CreateReleaseRequestCommand>
{
    public CreateReleaseRequestCommandValidator()
    {
        RuleFor(v => v.Location).MaximumLength(500);
        RuleFor(v => v.Notes).MaximumLength(2000);
    }
}

public class CreateReleaseRequestCommandHandler : IRequestHandler<CreateReleaseRequestCommand, int>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public CreateReleaseRequestCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task<int> Handle(CreateReleaseRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = new ReleaseRequest
        {
            Location = request.Location,
            ReleaseDate = request.ReleaseDate,
            ReleaseTypeId = request.ReleaseTypeId,
            Notes = request.Notes
        };

        _db.ReleaseRequests.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Create", "ReleaseRequest", entity.ReleaseRequestId, _user.UserName, cancellationToken);
        return entity.ReleaseRequestId;
    }
}
