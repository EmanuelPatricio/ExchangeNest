using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Shared;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Users.ChangePassword;
internal sealed class ChangePasswordUserCommandHandler : ICommandHandler<ChangePasswordUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangePasswordUserCommand command, CancellationToken cancellationToken = default)
    {
        var userEmail = EncodePassword.DecodeFrom64(command.Token);

        var emailResult = Email.Create(userEmail);

        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        if (await _userRepository.IsEmailUniqueAsync(emailResult.Value))
        {
            return UserErrors.NotFoundEmail;
        }

        var password = new UserPassword(command.NewPassword);

        await _userRepository.ChangePasswordAsync(emailResult.Value, password);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
