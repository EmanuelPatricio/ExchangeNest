using Domain.Abstractions;

namespace Application.Shared.Queries.GetEmailConfigurations;
public static class GetEmailConfigurationErrors
{
    public static Error SettingsNotConfigured = new(
        "EmailConfiguration.SettingsNotConfured",
        "The email settings are not stablished, please contact with support.");

    public static Error SenderMailNotStablished = new(
        "EmailConfiguration.SenderMailNotStablished",
        "The sender mail is not stablished, please contact with support.");

    public static Error SenderPasswordNotStablished = new(
        "EmailConfiguration.SenderPasswordNotStablished",
        "The sender password is not stablished, please contact with support.");

    public static Error EmailTemplateNotStablished = new(
        "EmailConfiguration.EmailTemplate",
        "The email template is not stablished, please contact with support.");
}
