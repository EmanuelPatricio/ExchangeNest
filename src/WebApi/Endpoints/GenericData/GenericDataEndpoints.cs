using Application.GenericData.Get;
using Carter;
using Domain.GenericStatuses;
using Microsoft.AspNetCore.Http.HttpResults;
using static Domain.Shared.Enums;

namespace WebApi.Endpoints.GenericData;

public class GenericDataEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/genericData").WithTags("Generic Data");

        group.MapGet("/statuses", GetStatuses);
        group.MapGet("/roles", GetRoles);
        group.MapGet("/organization-types", GetOrganizationTypes);
        group.MapGet("/countries", GetCountries);
        group.MapGet("/document-types", GetDocumentTypes);
    }

    public static async Task<Results<Ok<List<GetGenericStatusesResponse>>, NotFound, BadRequest<string>>> GetStatuses(IGenericStatusRepository genericStatusRepository)
    {
        try
        {
            var genericStatusesResponse = new List<GetGenericStatusesResponse>();

            var genericStatusesList = await genericStatusRepository.GetAll();

            genericStatusesList = genericStatusesList.OrderBy(x => x.Concept).ThenBy(x => x.Order).ToList();

            if (genericStatusesList is null || genericStatusesList.Count == 0)
            {
                return TypedResults.NotFound();
            }

            foreach (var genericStatus in genericStatusesList)
            {
                var response = new GetGenericStatusesResponse(genericStatus.Id, genericStatus.Description);

                genericStatusesResponse.Add(response);
            }

            return TypedResults.Ok(genericStatusesResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static Results<Ok<List<GetRolesResponse>>, NotFound, BadRequest<string>> GetRoles()
    {
        try
        {
            var rolesResponse = new List<GetRolesResponse>();

            foreach (int id in Enum.GetValues(typeof(Roles)))
            {
                var response = new GetRolesResponse(id, Enum.GetName(typeof(Roles), id) ?? string.Empty);

                rolesResponse.Add(response);
            }

            return TypedResults.Ok(rolesResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static Results<Ok<List<GetOrganizationTypesResponse>>, NotFound, BadRequest<string>> GetOrganizationTypes()
    {
        try
        {
            var rolesResponse = new List<GetOrganizationTypesResponse>();

            foreach (int id in Enum.GetValues(typeof(OrganizationTypes)))
            {
                var response = new GetOrganizationTypesResponse(id, Enum.GetName(typeof(OrganizationTypes), id) ?? string.Empty);

                rolesResponse.Add(response);
            }

            return TypedResults.Ok(rolesResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static Results<Ok<List<GetCountriesResponse>>, NotFound, BadRequest<string>> GetCountries()
    {
        try
        {
            var countriesResponse = new List<GetCountriesResponse>();

            foreach (int id in Enum.GetValues(typeof(Countries)))
            {
                var response = new GetCountriesResponse(id, Enum.GetName(typeof(Countries), id) ?? string.Empty);

                countriesResponse.Add(response);
            }

            return TypedResults.Ok(countriesResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static Results<Ok<List<GetDocumentTypesResponse>>, NotFound, BadRequest<string>> GetDocumentTypes()
    {
        try
        {
            var rolesResponse = new List<GetDocumentTypesResponse>();

            foreach (var entry in DocumentTypesDisplay)
            {
                rolesResponse.Add(new(entry.Key, entry.Value));
            }

            return TypedResults.Ok(rolesResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }
}
