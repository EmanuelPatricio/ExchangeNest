using Application.Abstractions.Email;
using Application.Shared.Queries.GetEmailConfigurations;
using Domain.Abstractions;
using MediatR;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Abstractions.Email;
internal sealed class EmailSender : IEmailSender
{
    private readonly ISender _sender;

    public EmailSender(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result> SendEmailAsync(EmailRequest email)
    {
        var query = new GetEmailConfigurationQuery();

        var emailConfigurations = await _sender.Send(query);

        if (emailConfigurations is null)
        {
            return GetEmailConfigurationErrors.SettingsNotConfigured;
        }

        if (emailConfigurations.IsFailure)
        {
            return emailConfigurations.Error;
        }

        var templateWithContent = emailConfigurations.Value.EmailTemplate.Replace("[Contenido]", email.Message);

        var client = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(emailConfigurations.Value.SenderMail, emailConfigurations.Value.SenderPassword),
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailConfigurations.Value.SenderMail),
            Subject = email.Subject,
            IsBodyHtml = true,
            Body = templateWithContent,
        };

        mailMessage.To.Add(new MailAddress(email.To));

        await client.SendMailAsync(mailMessage);

        return Result.Success();
    }
}
