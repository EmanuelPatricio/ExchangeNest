using Application.Abstractions.Messaging;

namespace Application.Applications.Close;
public sealed record CloseApplicationCommand(int Id, string Reason) : ICommand;