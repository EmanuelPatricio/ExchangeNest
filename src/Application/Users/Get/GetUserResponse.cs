namespace Application.Users.Get;
public sealed record GetUserResponse(
    int Id,
    string FirstName,
    string LastName,
    string Nic,
    string Email,
    DateTime BirthDate,
    int RoleId,
    int StatusId,
    int OrganizationId,
    int CountryId,
    string? ImageUrl);