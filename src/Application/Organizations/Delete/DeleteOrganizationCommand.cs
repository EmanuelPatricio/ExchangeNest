using Application.Abstractions.Messaging;

namespace Application.Organizations.Delete;
public sealed record DeleteOrganizationCommand(int Id) : ICommand;