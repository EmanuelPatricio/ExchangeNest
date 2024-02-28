using Application.Abstractions.Messaging;

namespace Application.Users.ChangePassword;
public sealed record ChangePasswordUserCommand(
    string Token,
    string NewPassword) : ICommand;