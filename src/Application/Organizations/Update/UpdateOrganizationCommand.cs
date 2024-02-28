using Application.Abstractions.Messaging;

namespace Application.Organizations.Update;
public sealed record UpdateOrganizationCommand(
    int Id,
    string Name,
    string Description,
    string Email,
    string PhoneNumber,
    string Address,
    int OrganizationTypeId,
    int StatusId,
    string? ImageUrl) : ICommand;
