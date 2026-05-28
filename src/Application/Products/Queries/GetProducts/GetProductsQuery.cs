namespace SCMApp3.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<List<ProductDto>>;

public class ProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly SCMDbContext _db;

    public GetProductsQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        => await _db.Products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Description = p.Description })
            .ToListAsync(cancellationToken);
}
