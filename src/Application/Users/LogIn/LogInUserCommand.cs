using Application.Abstractions.Messaging;

namespace Application.Users.LogIn;
public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<LogInUserResponse>;