namespace WebApi.Endpoints.Configurations;

public sealed record UpdateConfigurationRequest(string SenderMail, string SenderPassword, string BaseTemplate, string ForgotPassword, string RegisterOrganization, string PublishApplication, string UpdateApplication, string RegisterUser, string CompletedApplication, string CancelledApplication);