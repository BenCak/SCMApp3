using SCMApp3.Application.ReleaseTypes.Queries.GetReleaseTypes;

namespace SCMApp3.Web.Endpoints;

public class ReleaseTypes : IEndpointGroup
{
    public static string? RoutePrefix => "/api/release-types";

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("", async (ISender sender) =>
            await sender.Send(new GetReleaseTypesQuery()))
            .WithName("GetReleaseTypes");
    }
}
