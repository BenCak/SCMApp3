namespace SCMApp3.Application.Common.Interfaces;

public interface IAuditService
{
    Task LogAsync(string action, string entityType, int entityId, string? performedBy = null, CancellationToken cancellationToken = default);
}
