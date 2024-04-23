using Application.Abstractions.Email;
using Application.Shared.Queries.GetEmailConfigurations;
using Application.Shared.Queries.GetEmailMessage;
using Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using static Domain.Shared.Enums;

namespace Infrastructure.Abstractions.Email;
internal sealed class EmailSender : IEmailSender
{
    private readonly ISender _sender;
    private readonly IConfiguration _configuration;

    public EmailSender(ISender sender, IConfiguration configuration)
    {
        _sender = sender;
        _configuration = configuration;
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

        var emailTemplate = await GetEmailHtmlFileData(EmailHtmlFile.BaseTemplate);

        if (emailTemplate.IsFailure)
        {
            return emailTemplate.Error;
        }

        var templateWithContent = emailTemplate.Value.Replace("[Contenido]", email.Message);

        using var client = new SmtpClient("smtp.office365.com", 587)
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

    public async Task<Result<string>> GetEmailHtmlFileData(EmailHtmlFile file)
    {
        var query = new GetEmailMessageQuery(file);

        var message = await _sender.Send(query);

        if (message is null)
        {
            return Result.Failure<string>(GetEmailConfigurationErrors.SettingsNotConfigured);
        }

        if (message.IsFailure)
        {
            return Result.Failure<string>(message.Error);
        }

        if (string.IsNullOrWhiteSpace(message.Value))
        {
            return Result.Failure<string>(Domain.Abstractions.Error.NullValue);
        }

        return Result.Success(message.Value);
    }
}
