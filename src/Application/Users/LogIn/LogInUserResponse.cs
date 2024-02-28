namespace Application.Users.LogIn;
public sealed record LogInUserResponse(
    int Id,
    string FirstName,
    string LastName,
    string Nic,
    string Email,
    DateTime BirthDate,
    string RoleText,
    int StatusId,
    int OrganizationId,
    int CountryId,
    string AccessToken);