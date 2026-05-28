using SCMApp3.Application.Segments.Commands.CreateSegment;
using SCMApp3.Application.Segments.Commands.UpdateSegmentStatus;
using SCMApp3.Application.Segments.Queries.GetSegments;
using SCMApp3.Domain.Enums;

namespace SCMApp3.Web.Endpoints;

public class Segments : IEndpointGroup
{
    public static string? RoutePrefix => "/api/segments";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (int systemVersionId, ISender sender) =>
            await sender.Send(new GetSegmentsQuery(systemVersionId)))
            .WithName("GetSegments");

        group.MapPost("", async (CreateSegmentCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/segments/{id}", new { id });
        }).WithName("CreateSegment");

        group.MapPatch("{id:int}/status", async (int id, Status newStatus, ISender sender) =>
        {
            await sender.Send(new UpdateSegmentStatusCommand(id, newStatus));
            return Results.NoContent();
        }).WithName("UpdateSegmentStatus");
    }
}
