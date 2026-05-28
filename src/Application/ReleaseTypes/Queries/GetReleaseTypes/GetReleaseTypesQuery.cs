namespace SCMApp3.Application.ReleaseTypes.Queries.GetReleaseTypes;

public record GetReleaseTypesQuery : IRequest<List<ReleaseTypeDto>>;

public class ReleaseTypeDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}

public class GetReleaseTypesQueryHandler : IRequestHandler<GetReleaseTypesQuery, List<ReleaseTypeDto>>
{
    private readonly SCMDbContext _db;

    public GetReleaseTypesQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<ReleaseTypeDto>> Handle(GetReleaseTypesQuery request, CancellationToken cancellationToken)
        => await _db.ReleaseTypes
            .OrderBy(t => t.Name)
            .Select(t => new ReleaseTypeDto
            {
                Id = t.ReleaseTypeId,
                Name = t.Name,
                Description = t.Description
            })
            .ToListAsync(cancellationToken);
}
