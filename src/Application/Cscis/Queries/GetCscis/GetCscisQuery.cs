namespace SCMApp3.Application.Cscis.Queries.GetCscis;

public record GetCscisQuery(int SegmentId) : IRequest<List<CsciDto>>;

public class CsciDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string VersionNumber { get; init; } = null!;
    public string Status { get; init; } = null!;
    public DateTime? ReleasedDate { get; init; }
    public string? ReleaseLocation { get; init; }
    public string? ChargeNumber { get; init; }
    public string? PocName { get; init; }
    public string? PocEmail { get; init; }
    public string? PocPhone { get; init; }
    public int? ParentId { get; init; }
}

public class GetCscisQueryHandler : IRequestHandler<GetCscisQuery, List<CsciDto>>
{
    private readonly SCMDbContext _db;

    public GetCscisQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<CsciDto>> Handle(GetCscisQuery request, CancellationToken cancellationToken)
        => await _db.Segments
            .Where(s => s.Id == request.SegmentId)
            .SelectMany(s => s.Cscis)
            .OrderBy(c => c.Name)
            .Select(c => new CsciDto
            {
                Id = c.Id,
                Name = c.Name,
                VersionNumber = c.VersionNumber,
                Status = c.Status,
                ReleasedDate = c.ReleasedDate,
                ReleaseLocation = c.ReleaseLocation,
                ChargeNumber = c.ChargeNumber,
                PocName = c.PocName,
                PocEmail = c.PocEmail,
                PocPhone = c.PocPhone,
                ParentId = c.ParentId
            })
            .ToListAsync(cancellationToken);
}
