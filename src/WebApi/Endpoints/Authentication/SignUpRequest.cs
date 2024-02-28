namespace WebApi.Endpoints.Authentication;

public sealed record SignUpRequest(
    string FirstName,
    string LastName,
    string Nic,
    string Email,
    string Password,
    DateTime BirthDate,
    string? ImageUrl,
    int RoleId,
    int StatusId,
    int OrganizationId,
    int CountryId,
    string? Token);