using System;
using System.Collections.Generic;

namespace SCMApp3.Data;

public partial class ReleaseRequest
{
    public int ReleaseRequestId { get; set; }

    public string? Location { get; set; }

    public string? ReleaseDate { get; set; }

    public int? ReleaseTypeId { get; set; }

    public string? Notes { get; set; }
}
