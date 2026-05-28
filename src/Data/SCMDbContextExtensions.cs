using Microsoft.EntityFrameworkCore;

namespace SCMApp3.Data;

// Partial class extensions — safe to edit, will NOT be overwritten by EF Core Power Tools.
// Use this file to add relationships or config that the scaffolder missed (e.g. missing FK constraints in the DB).
public partial class SCMDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReleaseRequest>()
            .HasOne(r => r.ReleaseType)
            .WithMany(t => t.ReleaseRequests)
            .HasForeignKey(r => r.ReleaseTypeId);
    }
}
