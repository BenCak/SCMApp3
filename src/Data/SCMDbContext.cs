using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SCMApp3.Data;

public partial class SCMDbContext : DbContext
{
    public SCMDbContext(DbContextOptions<SCMDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Csci> Cscis { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ReleaseRequest> ReleaseRequests { get; set; }

    public virtual DbSet<ReleaseType> ReleaseTypes { get; set; }

    public virtual DbSet<Segment> Segments { get; set; }

    public virtual DbSet<System> Systems { get; set; }

    public virtual DbSet<SystemVersion> SystemVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Csci>(entity =>
        {
            entity.HasIndex(e => e.ParentId, "IX_Cscis_ParentId");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);
        });

        modelBuilder.Entity<ReleaseRequest>(entity =>
        {
            entity.ToTable("ReleaseRequest");

            entity.Property(e => e.ReleaseRequestId).HasColumnName("ReleaseRequestID");
            entity.Property(e => e.ReleaseTypeId).HasColumnName("ReleaseTypeID");
        });

        modelBuilder.Entity<ReleaseType>(entity =>
        {
            entity.ToTable("ReleaseType");

            entity.Property(e => e.ReleaseTypeId)
                .ValueGeneratedNever()
                .HasColumnName("ReleaseTypeID");
        });

        modelBuilder.Entity<Segment>(entity =>
        {
            entity.HasIndex(e => e.ParentId, "IX_Segments_ParentId");

            entity.HasIndex(e => e.SystemVersionId, "IX_Segments_SystemVersionId");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);

            entity.HasOne(d => d.SystemVersion).WithMany(p => p.Segments).HasForeignKey(d => d.SystemVersionId);

            entity.HasMany(d => d.Cscis).WithMany(p => p.Segments)
                .UsingEntity<Dictionary<string, object>>(
                    "SegmentCsci",
                    r => r.HasOne<Csci>().WithMany().HasForeignKey("CsciId"),
                    l => l.HasOne<Segment>().WithMany().HasForeignKey("SegmentId"),
                    j =>
                    {
                        j.HasKey("SegmentId", "CsciId");
                        j.ToTable("SegmentCscis");
                        j.HasIndex(new[] { "CsciId" }, "IX_SegmentCscis_CsciId");
                    });
        });

        modelBuilder.Entity<System>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Systems_CustomerId");

            entity.HasIndex(e => e.ParentId, "IX_Systems_ParentId");

            entity.HasIndex(e => e.ProductId, "IX_Systems_ProductId");

            entity.HasOne(d => d.Customer).WithMany(p => p.Systems).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);

            entity.HasOne(d => d.Product).WithMany(p => p.Systems).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<SystemVersion>(entity =>
        {
            entity.HasIndex(e => e.ParentId, "IX_SystemVersions_ParentId");

            entity.HasIndex(e => e.SystemId, "IX_SystemVersions_SystemId");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);

            entity.HasOne(d => d.System).WithMany(p => p.SystemVersions).HasForeignKey(d => d.SystemId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
