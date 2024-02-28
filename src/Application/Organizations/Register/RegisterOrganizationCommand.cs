using Application.Abstractions.Messaging;

namespace Application.Organizations.Register;
public sealed record RegisterOrganizationCommand(
    int Id,
    string Name,
    string Description,
    string Email,
    string PhoneNumber,
    string Address,
    int OrganizationTypeId,
    int StatusId,
    string? ImageUrl,
    string Url) : ICommand;
