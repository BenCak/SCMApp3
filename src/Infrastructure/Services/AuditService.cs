using Microsoft.Extensions.Logging;
using SCMApp3.Application.Common.Interfaces;

namespace SCMApp3.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly ILogger<AuditService> _logger;

    public AuditService(ILogger<AuditService> logger) => _logger = logger;

    public Task LogAsync(string action, string entityType, int entityId, string? performedBy = null, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("AUDIT | {Action} | {EntityType} #{EntityId} | by {User}",
            action, entityType, entityId, performedBy ?? "unknown");
        return Task.CompletedTask;
    }
}
