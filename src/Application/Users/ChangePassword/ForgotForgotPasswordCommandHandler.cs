using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Users.Shared;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;
using static Domain.Shared.Enums;

namespace Application.Users.ChangePassword;
internal sealed class ForgotForgotPasswordCommandHandler : ICommandHandler<ForgotForgotPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;

    public ForgotForgotPasswordCommandHandler(IUserRepository userRepository, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
    }

    public async Task<Result> Handle(ForgotForgotPasswordCommand command, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        if (await _userRepository.IsEmailUniqueAsync(emailResult.Value))
        {
            return Email.NotFound;
        }

        var user = await _userRepository.GetByEmail(emailResult.Value);

        if (user is null)
        {
            return UserErrors.NotFoundEmail;
        }

        var token = EncodePassword.EncodeToBase64(command.Email);

        var emailMessage = await _emailSender.GetEmailHtmlFileData(EmailHtmlFile.ForgotPassword);

        if (emailMessage.IsFailure)
        {
            return Email.NotSended;
        }

        var message = emailMessage.Value.Replace("[User name]", user.FirstName.Value);
        message = message.Replace("[url]", $"{command.Url}token={token}");

        var result = await _emailSender.SendEmailAsync(new EmailRequest(
            To: command.Email,
            Subject: "Password reset for your account",
            Message: message));

        if (result.IsFailure)
        {
            return Email.NotSended;
        }

        return Result.Success();
    }
}
