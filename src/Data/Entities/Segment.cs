using System;
using System.Collections.Generic;

namespace SCMApp3.Data;

public partial class Segment
{
    public int Id { get; set; }

    public int SystemVersionId { get; set; }

    public string Name { get; set; } = null!;

    public string VersionNumber { get; set; } = null!;

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

    public virtual ICollection<Segment> InverseParent { get; set; } = new List<Segment>();

    public virtual Segment? Parent { get; set; }

    public virtual SystemVersion SystemVersion { get; set; } = null!;

    public virtual ICollection<Csci> Cscis { get; set; } = new List<Csci>();
}
