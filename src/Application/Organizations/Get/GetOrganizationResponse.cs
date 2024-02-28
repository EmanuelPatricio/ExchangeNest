namespace Application.Organizations.Get;
public sealed record GetOrganizationResponse(
    int Id,
    string Name,
    string Description,
    string Email,
    string PhoneNumber,
    string Address,
    int OrganizationTypeId,
    int StatusId,
    string? ImageUrl);
