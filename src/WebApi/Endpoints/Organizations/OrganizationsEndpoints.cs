using Application.Organizations.Delete;
using Application.Organizations.Get;
using Application.Organizations.Register;
using Application.Organizations.Update;
using Application.Shared.Queries.GetNewId;
using Carter;
using Domain.Organizations;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using static Domain.Shared.Enums;

namespace WebApi.Endpoints.Organizations;

public class OrganizationsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/organization").WithTags("Organization").RequireAuthorization();

        group.MapPost("", RegisterOrganization);
        group.MapGet("/{organizationId:int}", GetById);
        group.MapGet("", GetAll);
        group.MapPut("", Update);
        group.MapDelete("/{organizationId:int}", Delete);
    }

    public static async Task<Results<Created, UnprocessableEntity<string>, BadRequest<string>>> RegisterOrganization(RegisterOrganizationRequest request, ISender sender)
    {
        try
        {
            var query = new GetNewIdQuery("Id", "Organizations");

            var id = await sender.Send(query);

            if (id is null)
            {
                return TypedResults.UnprocessableEntity("Couldn't get id for the new organization");
            }

            var command = new RegisterOrganizationCommand(
                id.Value,
                request.Name,
                request.Description,
                request.Email,
                request.PhoneNumber,
                request.Address,
                request.OrganizationTypeId,
                request.StatusId,
                request.ImageUrl,
                request.Url);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return TypedResults.UnprocessableEntity(result.Error.Name);
            }

            return TypedResults.Created();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok<GetOrganizationResponse>, NotFound, BadRequest<string>>> GetById(int organizationId, IOrganizationRepository organizationRepository)
    {
        try
        {
            var organization = await organizationRepository.GetById(new OrganizationId(organizationId));

            if (organization is null)
            {
                return TypedResults.NotFound();
            }

            var response = new GetOrganizationResponse(organization.Id.Value, organization.Name.Value, organization.Description.Value, organization.Email.Value, organization.PhoneNumber.Value, organization.Address.Value, organization.OrganizationTypeId, organization.StatusId, organization.ImageUrl?.Value);

            return TypedResults.Ok(response);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok<List<GetOrganizationResponse>>, NotFound, BadRequest<string>>> GetAll(IOrganizationRepository organizationRepository)
    {
        try
        {
            var organizationsResponse = new List<GetOrganizationResponse>();

            var organizationsList = await organizationRepository.GetAll();

            if (organizationsList is null || organizationsList.Count == 0)
            {
                return TypedResults.NotFound();
            }

            organizationsList = organizationsList.Where(x => x.StatusId != (int)Statuses.Deleted).ToList();

            foreach (var organization in organizationsList)
            {
                var response = new GetOrganizationResponse(organization.Id.Value, organization.Name.Value, organization.Description.Value, organization.Email.Value, organization.PhoneNumber.Value, organization.Address.Value, organization.OrganizationTypeId, organization.StatusId, organization.ImageUrl?.Value);

                organizationsResponse.Add(response);
            }

            return TypedResults.Ok(organizationsResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> Update(UpdateOrganizationRequest request,
        ISender sender)
    {
        try
        {
            var command = new UpdateOrganizationCommand(
                request.Id,
                request.Name,
                request.Description,
                request.Email,
                request.PhoneNumber,
                request.Address,
                request.OrganizationTypeId,
                request.StatusId,
                request.ImageUrl);

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

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> Delete(int organizationId, ISender sender)
    {
        try
        {
            var command = new DeleteOrganizationCommand(organizationId);

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
