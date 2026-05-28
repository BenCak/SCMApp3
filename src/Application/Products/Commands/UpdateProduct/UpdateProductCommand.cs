namespace SCMApp3.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(int Id, string Name, string? Description) : IRequest;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(200).NotEmpty();
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly SCMDbContext _db;
    private readonly IAuditService _audit;
    private readonly IUser _user;

    public UpdateProductCommandHandler(SCMDbContext db, IAuditService audit, IUser user)
    {
        _db = db; _audit = audit; _user = user;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.Products.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _db.SaveChangesAsync(cancellationToken);
        await _audit.LogAsync("Update", "Product", entity.Id, _user.UserName, cancellationToken);
    }
}
