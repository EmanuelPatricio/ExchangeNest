using Application.Configurations.Update;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Endpoints.Configurations;

public class ConfigurationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/configuration").WithTags("Configuration").RequireAuthorization();

        group.MapPut("", Update);
    }

    public static async Task<Results<Ok, BadRequest<string>, UnprocessableEntity<string>>> Update(UpdateConfigurationRequest request, ISender sender)
    {
        try
        {
            var command = new UpdateConfigurationCommand(request.SenderMail, request.SenderPassword, request.EmailTemplate);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return TypedResults.UnprocessableEntity(result.Error.Name);
            }

            return TypedResults.Ok();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }
}
