using System;
using System.Collections.Generic;

namespace SCMApp3.Data;

public partial class SystemVersion
{
    public int Id { get; set; }

    public int SystemId { get; set; }

    public string VersionNumber { get; set; } = null!;

    public string? SvdPath { get; set; }

    public string? SvmPath { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ReleasedDate { get; set; }

    public string? PocName { get; set; }

    public string? PocEmail { get; set; }

    public string? PocPhone { get; set; }

    public int? ParentId { get; set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual ICollection<SystemVersion> InverseParent { get; set; } = new List<SystemVersion>();

    public virtual SystemVersion? Parent { get; set; }

    public virtual ICollection<Segment> Segments { get; set; } = new List<Segment>();

    public virtual System System { get; set; } = null!;
}
