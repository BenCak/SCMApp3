namespace SCMApp3.Application.Systems.Queries.GetSystems;

public record GetSystemsQuery(int? CustomerId = null, int? ProductId = null) : IRequest<List<SystemDto>>;

public class SystemDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = null!;
    public int ProductId { get; init; }
    public string ProductName { get; init; } = null!;
    public string? PocName { get; init; }
    public string? PocEmail { get; init; }
    public string? PocPhone { get; init; }
    public int? ParentId { get; init; }
}

public class GetSystemsQueryHandler : IRequestHandler<GetSystemsQuery, List<SystemDto>>
{
    private readonly SCMDbContext _db;

    public GetSystemsQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<SystemDto>> Handle(GetSystemsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Systems
            .Include(s => s.Customer)
            .Include(s => s.Product)
            .AsQueryable();

        if (request.CustomerId.HasValue)
            query = query.Where(s => s.CustomerId == request.CustomerId);
        if (request.ProductId.HasValue)
            query = query.Where(s => s.ProductId == request.ProductId);

        return await query
            .OrderBy(s => s.Customer.Name).ThenBy(s => s.Product.Name)
            .Select(s => new SystemDto
            {
                Id = s.Id,
                CustomerId = s.CustomerId,
                CustomerName = s.Customer.Name,
                ProductId = s.ProductId,
                ProductName = s.Product.Name,
                PocName = s.PocName,
                PocEmail = s.PocEmail,
                PocPhone = s.PocPhone,
                ParentId = s.ParentId
            })
            .ToListAsync(cancellationToken);
    }
}
