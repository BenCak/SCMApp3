namespace MudBlazorClient.Models;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Abbreviation { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class SystemDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? PocName { get; set; }
    public string? PocEmail { get; set; }
    public string? PocPhone { get; set; }
    public int? ParentId { get; set; }
}

public class SystemVersionDto
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
}

public class SegmentDto
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
}

public class ReleaseTypeDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class ReleaseRequestDto
{
    public int Id { get; set; }
    public string? Location { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Notes { get; set; }
    public int? ReleaseTypeId { get; set; }
    public string? ReleaseTypeName { get; set; }
}

public class CsciDto
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
}
