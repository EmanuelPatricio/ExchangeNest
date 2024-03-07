using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Users.Shared;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;

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

        var token = EncodePassword.EncodeToBase64(command.Email);

        var result = await _emailSender.SendEmailAsync(new EmailRequest(command.Email, "Password forgotten", $"<a href=\"{command.Url}token={token}\" target=\"_blank\">Click here</a>"));

        if (result.IsFailure)
        {
            return Email.NotSended;
        }

        return Result.Success();
    }
}
