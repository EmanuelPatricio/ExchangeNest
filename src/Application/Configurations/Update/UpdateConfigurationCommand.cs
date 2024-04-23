using Application.Abstractions.Messaging;

namespace Application.Configurations.Update;
public sealed record UpdateConfigurationCommand(string SenderMail, string SenderPassword, string BaseTemplate, string ForgotPassword, string RegisterOrganization, string PublishApplication, string UpdateApplication, string RegisterUser, string CompletedApplication, string CancelledApplication) : ICommand;