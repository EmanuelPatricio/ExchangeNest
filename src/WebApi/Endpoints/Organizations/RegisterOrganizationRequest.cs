namespace WebApi.Endpoints.Organizations;

public sealed record RegisterOrganizationRequest(
    string Name,
    string Description,
    string Email,
    string PhoneNumber,
    string Address,
    int OrganizationTypeId,
    int StatusId,
    string? ImageUrl,
    string Url);
