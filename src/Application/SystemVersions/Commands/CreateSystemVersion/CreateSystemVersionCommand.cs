namespace SCMApp3.Application.SystemVersions.Commands.CreateSystemVersion;

public record CreateSystemVersionCommand(
    int SystemId,
    string VersionNumber,
    string? SvdPath,
    string? SvmPath,
    string? PocName,
    string? PocEmail,
    string? PocPhone,
    int? ParentId = null) : IRequest<int>;

public class CreateSystemVersionCommandValidator : AbstractValidator<CreateSystemVersionCommand>
{
    public CreateSystemVersionCommandValidator()
    {
        RuleFor(v => v.SystemId).GreaterThan(0);
        RuleFor(v => v.VersionNumber).NotEmpty()
            .Matches(@"^\d+\.\d+\.\d+").WithMessage("VersionNumber must be in Major.Minor.Patch format.");
        RuleFor(v => v.PocEmail).EmailAddress().When(v => !string.IsNullOrWhiteSpace(v.PocEmail));
    }
}

public class CreateSystemVersionCommandHandler : IRequestHandler<CreateSystemVersionCommand, int>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public CreateSystemVersionCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task<int> Handle(CreateSystemVersionCommand request, CancellationToken cancellationToken)
    {
        var entity = new SystemVersion
        {
            SystemId = request.SystemId,
            VersionNumber = request.VersionNumber,
            SvdPath = request.SvdPath,
            SvmPath = request.SvmPath,
            ParentId = request.ParentId,
            Status = Status.Pending.ToString(),
            PocName = request.PocName,
            PocEmail = request.PocEmail,
            PocPhone = request.PocPhone
        };

        _db.SystemVersions.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Create", "SystemVersion", entity.Id, _user.UserName, cancellationToken);
        return entity.Id;
    }
}
