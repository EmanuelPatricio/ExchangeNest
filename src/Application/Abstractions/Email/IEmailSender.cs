using Domain.Abstractions;

namespace Application.Abstractions.Email;
public interface IEmailSender
{
    Task<Result> SendEmailAsync(EmailRequest email);
}
