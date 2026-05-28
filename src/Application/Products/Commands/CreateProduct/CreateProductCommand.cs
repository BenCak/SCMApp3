namespace SCMApp3.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(string Name, string? Description) : IRequest<int>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
    }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public CreateProductCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product { Name = request.Name, Description = request.Description };
        _db.Products.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Create", "Product", entity.Id, _user.UserName, cancellationToken);
        return entity.Id;
    }
}
