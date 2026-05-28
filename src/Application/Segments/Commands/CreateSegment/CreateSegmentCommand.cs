namespace SCMApp3.Application.Segments.Commands.CreateSegment;

public record CreateSegmentCommand(
    int SystemVersionId,
    string Name,
    string VersionNumber,
    string? PocName,
    string? PocEmail,
    string? PocPhone,
    int? ParentId = null) : IRequest<int>;

public class CreateSegmentCommandValidator : AbstractValidator<CreateSegmentCommand>
{
    public CreateSegmentCommandValidator()
    {
        RuleFor(v => v.SystemVersionId).GreaterThan(0);
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.VersionNumber).NotEmpty()
            .Matches(@"^\d+\.\d+\.\d+").WithMessage("VersionNumber must be in Major.Minor.Patch format.");
        RuleFor(v => v.PocEmail).EmailAddress().When(v => !string.IsNullOrWhiteSpace(v.PocEmail));
    }
}

public class CreateSegmentCommandHandler : IRequestHandler<CreateSegmentCommand, int>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public CreateSegmentCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task<int> Handle(CreateSegmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Segment
        {
            SystemVersionId = request.SystemVersionId,
            Name = request.Name,
            VersionNumber = request.VersionNumber,
            ParentId = request.ParentId,
            Status = Status.Pending.ToString(),
            PocName = request.PocName,
            PocEmail = request.PocEmail,
            PocPhone = request.PocPhone
        };

        _db.Segments.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Create", "Segment", entity.Id, _user.UserName, cancellationToken);
        return entity.Id;
    }
}
