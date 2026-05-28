namespace SCMApp3.Application.Cscis.Commands.CreateCsci;

public record CreateCsciCommand(
    int SegmentId,
    string Name,
    string VersionNumber,
    string? ReleaseLocation,
    string? ChargeNumber,
    string? PocName,
    string? PocEmail,
    string? PocPhone,
    int? ParentId = null) : IRequest<int>;

public class CreateCsciCommandValidator : AbstractValidator<CreateCsciCommand>
{
    public CreateCsciCommandValidator()
    {
        RuleFor(v => v.SegmentId).GreaterThan(0);
        RuleFor(v => v.Name).MaximumLength(100).NotEmpty();
        RuleFor(v => v.VersionNumber).NotEmpty()
            .Matches(@"^\d+\.\d+\.\d+").WithMessage("VersionNumber must be in Major.Minor.Patch format.");
        RuleFor(v => v.PocEmail).EmailAddress().When(v => !string.IsNullOrWhiteSpace(v.PocEmail));
    }
}

public class CreateCsciCommandHandler : IRequestHandler<CreateCsciCommand, int>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public CreateCsciCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task<int> Handle(CreateCsciCommand request, CancellationToken cancellationToken)
    {
        var segment = await _db.Segments
            .Include(s => s.Cscis)
            .FirstOrDefaultAsync(s => s.Id == request.SegmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Segment), request.SegmentId);

        var entity = new Csci
        {
            Name = request.Name,
            VersionNumber = request.VersionNumber,
            ReleaseLocation = request.ReleaseLocation,
            ChargeNumber = request.ChargeNumber,
            ParentId = request.ParentId,
            Status = Status.Pending.ToString(),
            PocName = request.PocName,
            PocEmail = request.PocEmail,
            PocPhone = request.PocPhone
        };

        segment.Cscis.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Create", "Csci", entity.Id, _user.UserName, cancellationToken);
        return entity.Id;
    }
}
