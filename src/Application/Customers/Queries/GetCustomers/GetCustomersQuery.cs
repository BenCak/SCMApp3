namespace SCMApp3.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery : IRequest<List<CustomerDto>>;

public class CustomerDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Abbreviation { get; init; }
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly SCMDbContext _db;

    public GetCustomersQueryHandler(SCMDbContext db) => _db = db;

    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        => await _db.Customers
            .OrderBy(c => c.Name)
            .Select(c => new CustomerDto { Id = c.Id, Name = c.Name, Abbreviation = c.Abbreviation })
            .ToListAsync(cancellationToken);
}
