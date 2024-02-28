namespace WebApi.Endpoints.Organizations;

public record UpdateOrganizationRequest(
    int Id,
    string Name,
    string Description,
    string Email,
    string PhoneNumber,
    string Address,
    int OrganizationTypeId,
    int StatusId,
    string? ImageUrl);