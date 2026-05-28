using SCMApp3.Application.ReleaseRequests.Commands.CreateReleaseRequest;
using SCMApp3.Application.ReleaseRequests.Queries.GetReleaseRequests;

namespace SCMApp3.Web.Endpoints;

public class ReleaseRequests : IEndpointGroup
{
    public static string? RoutePrefix => "/api/release-requests";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (ISender sender) =>
            await sender.Send(new GetReleaseRequestsQuery()))
            .WithName("GetReleaseRequests");

        group.MapPost("", async (CreateReleaseRequestCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/release-requests/{id}", new { id });
        }).WithName("CreateReleaseRequest");
    }
}
