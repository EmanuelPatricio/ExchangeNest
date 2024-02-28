using Application.Abstractions.Messaging;

namespace Application.Users.Create;
public sealed record RegisterUserCommand(
    int Id,
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
    string? Token) : ICommand;