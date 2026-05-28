namespace SCMApp3.Application.SystemVersions.Commands.UpdateSystemVersionStatus;

public record UpdateSystemVersionStatusCommand(int Id, Status NewStatus) : IRequest;

public class UpdateSystemVersionStatusCommandHandler : IRequestHandler<UpdateSystemVersionStatusCommand>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public UpdateSystemVersionStatusCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task Handle(UpdateSystemVersionStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.SystemVersions.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(SystemVersion), request.Id);

        var currentStatus = Enum.Parse<Status>(entity.Status);
        StatusTransitionValidator.ValidateOrThrow(currentStatus, request.NewStatus);

        entity.Status = request.NewStatus.ToString();
        if (request.NewStatus == Status.Released)
            entity.ReleasedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync($"StatusChange:{request.NewStatus}", "SystemVersion", entity.Id, _user.UserName, cancellationToken);
    }
}
