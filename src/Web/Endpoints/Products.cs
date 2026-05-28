using SCMApp3.Application.Products.Commands.CreateProduct;
using SCMApp3.Application.Products.Commands.UpdateProduct;
using SCMApp3.Application.Products.Queries.GetProducts;

namespace SCMApp3.Web.Endpoints;

public class Products : IEndpointGroup
{
    public static string? RoutePrefix => "/api/products";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (ISender sender) =>
            await sender.Send(new GetProductsQuery()))
            .WithName("GetProducts");

        group.MapPost("", async (CreateProductCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/products/{id}", new { id });
        }).WithName("CreateProduct");

        group.MapPut("{id:int}", async (int id, UpdateProductCommand command, ISender sender) =>
        {
            await sender.Send(command with { Id = id });
            return Results.NoContent();
        }).WithName("UpdateProduct");
    }
}
