using Application.Abstractions.Messaging;

namespace Application.Configurations.Update;
public sealed record UpdateConfigurationCommand(string SenderMail, string SenderPassword, string EmailTemplate) : ICommand;