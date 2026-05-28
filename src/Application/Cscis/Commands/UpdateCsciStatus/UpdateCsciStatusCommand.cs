namespace SCMApp3.Application.Cscis.Commands.UpdateCsciStatus;

public record UpdateCsciStatusCommand(int Id, Status NewStatus) : IRequest;

public class UpdateCsciStatusCommandHandler : IRequestHandler<UpdateCsciStatusCommand>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public UpdateCsciStatusCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task Handle(UpdateCsciStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.Cscis.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(Csci), request.Id);

        var currentStatus = Enum.Parse<Status>(entity.Status);
        StatusTransitionValidator.ValidateOrThrow(currentStatus, request.NewStatus);

        entity.Status = request.NewStatus.ToString();
        if (request.NewStatus == Status.Released)
            entity.ReleasedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync($"StatusChange:{request.NewStatus}", "Csci", entity.Id, _user.UserName, cancellationToken);
    }
}
