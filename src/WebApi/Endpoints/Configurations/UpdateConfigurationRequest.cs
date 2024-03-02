namespace WebApi.Endpoints.Configurations;

public sealed record UpdateConfigurationRequest(string SenderMail, string SenderPassword, string EmailTemplate);