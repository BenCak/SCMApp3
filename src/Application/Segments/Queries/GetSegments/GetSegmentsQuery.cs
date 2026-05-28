namespace SCMApp3.Application.Segments.Queries.GetSegments;

public record GetSegmentsQuery(int SystemVersionId) : IRequest<List<SegmentDto>>;

public class SegmentDto
{
    public int Id { get; init; }
    public int SystemVersionId { get; init; }
    public string Name { get; init; } = null!;
    public string VersionNumber { get; init; } = null!;
    public string Status { get; init; } = null!;
    public DateTime? ReleasedDate { get; init; }
    public string? PocName { get; init; }
    public string? PocEmail { get; init; }
    public string? PocPhone { get; init; }
    public int? ParentId { get; init; }
}

public class GetSegmentsQueryHandler : IRequestHandler<GetSegmentsQuery, List<SegmentDto>>
{
    private readonly SCMDbContext _db;

    public GetSegmentsQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<SegmentDto>> Handle(GetSegmentsQuery request, CancellationToken cancellationToken)
        => await _db.Segments
            .Where(s => s.SystemVersionId == request.SystemVersionId)
            .OrderBy(s => s.Name)
            .Select(s => new SegmentDto
            {
                Id = s.Id,
                SystemVersionId = s.SystemVersionId,
                Name = s.Name,
                VersionNumber = s.VersionNumber,
                Status = s.Status,
                ReleasedDate = s.ReleasedDate,
                PocName = s.PocName,
                PocEmail = s.PocEmail,
                PocPhone = s.PocPhone,
                ParentId = s.ParentId
            })
            .ToListAsync(cancellationToken);
}
