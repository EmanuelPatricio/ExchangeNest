using Domain.Abstractions;

namespace Domain.Configurations;
public sealed class Configuration : Entity<ConfigurationId>
{
    private Configuration() { }

    public Configuration(ConfigurationId id, string senderMail, string senderPassword, string baseTemplate, string forgotPassword, string registerOrganization, string publishApplication, string updateApplication, string registerUser, string completedApplication, string cancelledApplication) : base(id)
    {
        SenderMail = senderMail;
        SenderPassword = senderPassword;
        BaseTemplate = baseTemplate;
        ForgotPassword = forgotPassword;
        RegisterOrganization = registerOrganization;
        PublishApplication = publishApplication;
        UpdateApplication = updateApplication;
        RegisterUser = registerUser;
        CompletedApplication = completedApplication;
        CancelledApplication = cancelledApplication;
    }

    public string SenderMail { get; private set; }
    public string SenderPassword { get; private set; }
    public string BaseTemplate { get; private set; }
    public string ForgotPassword { get; private set; }
    public string RegisterOrganization { get; private set; }
    public string PublishApplication { get; private set; }
    public string UpdateApplication { get; private set; }
    public string RegisterUser { get; private set; }
    public string CompletedApplication { get; private set; }
    public string CancelledApplication { get; private set; }

    public static void Update(Configuration configuration, string senderMail, string senderPassword, string baseTemplate, string forgotPassword, string registerOrganization, string publishApplication, string updateApplication, string registerUser, string completedApplication, string cancelledApplication)
    {
        configuration.SenderMail = senderMail;
        configuration.SenderPassword = senderPassword;
        configuration.BaseTemplate = baseTemplate;
        configuration.ForgotPassword = forgotPassword;
        configuration.RegisterOrganization = registerOrganization;
        configuration.PublishApplication = publishApplication;
        configuration.UpdateApplication = updateApplication;
        configuration.RegisterUser = registerUser;
        configuration.CompletedApplication = completedApplication;
        configuration.CancelledApplication = cancelledApplication;
    }

    public static void DecodePassword(Configuration configuration, string decodedPassword)
    {
        configuration.SenderPassword = decodedPassword;
    }
}
