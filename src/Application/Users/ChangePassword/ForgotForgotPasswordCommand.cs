using Application.Abstractions.Messaging;

namespace Application.Users.ChangePassword;
public sealed record ForgotForgotPasswordCommand(string Email, string Url) : ICommand;
