using Application.Abstractions.Messaging;

namespace Application.Applications.Cancel;
public sealed record CancelApplicationCommand(int Id, string Reason) : ICommand;
