using SCMApp3.Application.Systems.Commands.CreateSystem;
using SCMApp3.Application.Systems.Queries.GetSystems;

namespace SCMApp3.Web.Endpoints;

public class Systems : IEndpointGroup
{
    public static string? RoutePrefix => "/api/systems";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (int? customerId, int? productId, ISender sender) =>
            await sender.Send(new GetSystemsQuery(customerId, productId)))
            .WithName("GetSystems");

        group.MapPost("", async (CreateSystemCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/systems/{id}", new { id });
        }).WithName("CreateSystem");
    }
}
