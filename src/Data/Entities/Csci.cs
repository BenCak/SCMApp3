using System;
using System.Collections.Generic;

namespace SCMApp3.Data;

public partial class Csci
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string VersionNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? ReleasedDate { get; set; }

    public string? ReleaseLocation { get; set; }

    public string? ChargeNumber { get; set; }

    public string? PocName { get; set; }

    public string? PocEmail { get; set; }

    public string? PocPhone { get; set; }

    public int? ParentId { get; set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual ICollection<Csci> InverseParent { get; set; } = new List<Csci>();

    public virtual Csci? Parent { get; set; }

    public virtual ICollection<Segment> Segments { get; set; } = new List<Segment>();
}
