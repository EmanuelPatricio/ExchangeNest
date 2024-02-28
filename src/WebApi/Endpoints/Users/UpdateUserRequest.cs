namespace WebApi.Endpoints.Users;

public record UpdateUserRequest(
    int Id,
    string FirstName,
    string LastName,
    string Nic,
    string Email,
    DateTime BirthDate,
    string? ImageUrl,
    int StatusId,
    int RoleId,
    int CountryId);