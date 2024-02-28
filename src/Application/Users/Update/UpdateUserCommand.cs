using Application.Abstractions.Messaging;

namespace Application.Users.Update;

public sealed record UpdateUserCommand(
    int Id,
    string FirstName,
    string LastName,
    string Nic,
    string Email,
    DateTime BirthDate,
    string? ImageUrl,
    int StatusId,
    int RoleId,
    int CountryId) : ICommand;