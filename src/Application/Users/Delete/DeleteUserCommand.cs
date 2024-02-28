using Application.Abstractions.Messaging;

namespace Application.Users.Delete;
public sealed record DeleteUserCommand(int Id) : ICommand;