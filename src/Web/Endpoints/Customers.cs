using SCMApp3.Application.Customers.Commands.CreateCustomer;
using SCMApp3.Application.Customers.Commands.UpdateCustomer;
using SCMApp3.Application.Customers.Queries.GetCustomers;

namespace SCMApp3.Web.Endpoints;

public class Customers : IEndpointGroup
{
    public static string? RoutePrefix => "/api/customers";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (ISender sender) =>
            await sender.Send(new GetCustomersQuery()))
            .WithName("GetCustomers");

        group.MapPost("", async (CreateCustomerCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/customers/{id}", new { id });
        }).WithName("CreateCustomer");

        group.MapPut("{id:int}", async (int id, UpdateCustomerCommand command, ISender sender) =>
        {
            await sender.Send(command with { Id = id });
            return Results.NoContent();
        }).WithName("UpdateCustomer");
    }
}
