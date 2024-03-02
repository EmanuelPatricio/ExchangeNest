using Domain.Abstractions;

namespace Domain.Configurations;
public sealed class Configuration : Entity<ConfigurationId>
{
    private Configuration() { }

    public Configuration(ConfigurationId id, string senderMail, string senderPassword, string emailTemplate) : base(id)
    {
        SenderMail = senderMail;
        SenderPassword = senderPassword;
        EmailTemplate = emailTemplate;
    }

    public string SenderMail { get; private set; }
    public string SenderPassword { get; private set; }
    public string EmailTemplate { get; private set; }

    public static void Update(Configuration configuration, string senderMail, string senderPassword, string emailTemplate)
    {
        configuration.SenderMail = senderMail;
        configuration.SenderPassword = senderPassword;
        configuration.EmailTemplate = emailTemplate;
    }

    public static void DecodePassword(Configuration configuration, string decodedPassword)
    {
        configuration.SenderPassword = decodedPassword;
    }
}
