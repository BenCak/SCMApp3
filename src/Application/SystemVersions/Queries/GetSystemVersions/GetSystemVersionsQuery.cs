namespace SCMApp3.Application.SystemVersions.Queries.GetSystemVersions;

public record GetSystemVersionsQuery(int SystemId) : IRequest<List<SystemVersionDto>>;

public class SystemVersionDto
{
    public int Id { get; init; }
    public int SystemId { get; init; }
    public string VersionNumber { get; init; } = null!;
    public string? SvdPath { get; init; }
    public string? SvmPath { get; init; }
    public string Status { get; init; } = null!;
    public DateTime? ReleasedDate { get; init; }
    public string? PocName { get; init; }
    public string? PocEmail { get; init; }
    public string? PocPhone { get; init; }
    public int? ParentId { get; init; }
}

public class GetSystemVersionsQueryHandler : IRequestHandler<GetSystemVersionsQuery, List<SystemVersionDto>>
{
    private readonly SCMDbContext _db;

    public GetSystemVersionsQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<SystemVersionDto>> Handle(GetSystemVersionsQuery request, CancellationToken cancellationToken)
        => await _db.SystemVersions
            .Where(sv => sv.SystemId == request.SystemId)
            .OrderByDescending(sv => sv.Created)
            .Select(sv => new SystemVersionDto
            {
                Id = sv.Id,
                SystemId = sv.SystemId,
                VersionNumber = sv.VersionNumber,
                SvdPath = sv.SvdPath,
                SvmPath = sv.SvmPath,
                Status = sv.Status,
                ReleasedDate = sv.ReleasedDate,
                PocName = sv.PocName,
                PocEmail = sv.PocEmail,
                PocPhone = sv.PocPhone,
                ParentId = sv.ParentId
            })
            .ToListAsync(cancellationToken);
}
