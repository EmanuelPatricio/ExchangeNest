using Domain.Abstractions;
using Domain.Shared;

namespace Application.Abstractions.Email;
public interface IEmailSender
{
    Task<Result<string>> GetEmailHtmlFileData(Enums.EmailHtmlFile file);
    Task<Result> SendEmailAsync(EmailRequest email);
}
