using SCMApp3.Application.Cscis.Commands.CreateCsci;
using SCMApp3.Application.Cscis.Commands.UpdateCsciStatus;
using SCMApp3.Application.Cscis.Queries.GetCscis;
using SCMApp3.Domain.Enums;

namespace SCMApp3.Web.Endpoints;

public class Cscis : IEndpointGroup
{
    public static string? RoutePrefix => "/api/cscis";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (int segmentId, ISender sender) =>
            await sender.Send(new GetCscisQuery(segmentId)))
            .WithName("GetCscis");

        group.MapPost("", async (CreateCsciCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/cscis/{id}", new { id });
        }).WithName("CreateCsci");

        group.MapPatch("{id:int}/status", async (int id, Status newStatus, ISender sender) =>
        {
            await sender.Send(new UpdateCsciStatusCommand(id, newStatus));
            return Results.NoContent();
        }).WithName("UpdateCsciStatus");
    }
}
