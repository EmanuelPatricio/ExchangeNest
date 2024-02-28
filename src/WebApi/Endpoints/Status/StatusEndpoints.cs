using Carter;

namespace WebApi.Endpoints;
public class StatusEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("status", () => Results.Ok()).WithTags("Health check");
    }
}
