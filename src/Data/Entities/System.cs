using System;
using System.Collections.Generic;

namespace SCMApp3.Data;

public partial class System
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public string? PocName { get; set; }

    public string? PocEmail { get; set; }

    public string? PocPhone { get; set; }

    public int? ParentId { get; set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<System> InverseParent { get; set; } = new List<System>();

    public virtual System? Parent { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<SystemVersion> SystemVersions { get; set; } = new List<SystemVersion>();
}
