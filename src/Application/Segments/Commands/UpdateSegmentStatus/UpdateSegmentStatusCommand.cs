namespace SCMApp3.Application.Segments.Commands.UpdateSegmentStatus;

public record UpdateSegmentStatusCommand(int Id, Status NewStatus) : IRequest;

public class UpdateSegmentStatusCommandHandler : IRequestHandler<UpdateSegmentStatusCommand>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public UpdateSegmentStatusCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task Handle(UpdateSegmentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.Segments.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(Segment), request.Id);

        var currentStatus = Enum.Parse<Status>(entity.Status);
        StatusTransitionValidator.ValidateOrThrow(currentStatus, request.NewStatus);

        entity.Status = request.NewStatus.ToString();
        if (request.NewStatus == Status.Released)
            entity.ReleasedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync($"StatusChange:{request.NewStatus}", "Segment", entity.Id, _user.UserName, cancellationToken);
    }
}
