using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SCMApp3.Application.Common.Interfaces;

namespace SCMApp3.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IUser _user;
    private readonly TimeProvider _dateTime;

    public AuditableEntityInterceptor(IUser user, TimeProvider dateTime)
    {
        _user = user;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = _dateTime.GetUtcNow();

        foreach (var entry in context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            var lastModified = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "LastModified");
            if (lastModified == null) continue;

            lastModified.CurrentValue = utcNow;
            var lastModifiedBy = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "LastModifiedBy");
            if (lastModifiedBy != null) lastModifiedBy.CurrentValue = _user.UserName;

            if (entry.State == EntityState.Added)
            {
                var created = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Created");
                if (created != null) created.CurrentValue = utcNow;
                var createdBy = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedBy");
                if (createdBy != null) createdBy.CurrentValue = _user.UserName;
            }
        }
    }
}
