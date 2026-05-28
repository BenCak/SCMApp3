using SCMApp3.Application.SystemVersions.Commands.CreateSystemVersion;
using SCMApp3.Application.SystemVersions.Commands.UpdateSystemVersionStatus;
using SCMApp3.Application.SystemVersions.Queries.GetSystemVersions;
using SCMApp3.Domain.Enums;

namespace SCMApp3.Web.Endpoints;

public class SystemVersions : IEndpointGroup
{
    public static string? RoutePrefix => "/api/system-versions";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (int systemId, ISender sender) =>
            await sender.Send(new GetSystemVersionsQuery(systemId)))
            .WithName("GetSystemVersions");

        group.MapPost("", async (CreateSystemVersionCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/system-versions/{id}", new { id });
        }).WithName("CreateSystemVersion");

        group.MapPatch("{id:int}/status", async (int id, Status newStatus, ISender sender) =>
        {
            await sender.Send(new UpdateSystemVersionStatusCommand(id, newStatus));
            return Results.NoContent();
        }).WithName("UpdateSystemVersionStatus");
    }
}
