namespace SCMApp3.Application.ReleaseRequests.Queries.GetReleaseRequests;

public record GetReleaseRequestsQuery : IRequest<List<ReleaseRequestDto>>;

public class ReleaseRequestDto
{
    public int Id { get; init; }
    public string? Location { get; init; }
    public string? ReleaseDate { get; init; }
    public string? Notes { get; init; }
    public int? ReleaseTypeId { get; init; }
    public string? ReleaseTypeName { get; init; }
}

public class GetReleaseRequestsQueryHandler : IRequestHandler<GetReleaseRequestsQuery, List<ReleaseRequestDto>>
{
    private readonly SCMDbContext _db;

    public GetReleaseRequestsQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<ReleaseRequestDto>> Handle(GetReleaseRequestsQuery request, CancellationToken cancellationToken)
        => await _db.ReleaseRequests
            .Include(r => r.ReleaseType)
            .Select(r => new ReleaseRequestDto
            {
                Id = r.ReleaseRequestId,
                Location = r.Location,
                ReleaseDate = r.ReleaseDate,
                Notes = r.Notes,
                ReleaseTypeId = r.ReleaseTypeId,
                ReleaseTypeName = r.ReleaseType != null ? r.ReleaseType.Name : null
            })
            .ToListAsync(cancellationToken);
}
