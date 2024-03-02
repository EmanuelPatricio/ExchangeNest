using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Users.Update;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        var id = new UserId(command.Id);

        if (await _userRepository.IsEmailUniqueAsync(emailResult.Value, id))
        {
            return UserErrors.NotFoundEmail;
        }

        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            return UserErrors.NotFound(command.Id);
        }

        var firstName = new UserFirstName(command.FirstName);
        var lastName = new UserLastName(command.LastName);
        var nic = new UserNIC(command.Nic);
        var birthDate = new UserBirthDate(command.BirthDate);
        var imageUrl = string.IsNullOrWhiteSpace(command.ImageUrl)
            ? null
            : new UserImageUrl(command.ImageUrl);

        User.Update(user, firstName, lastName, nic, emailResult.Value, birthDate, imageUrl, command.StatusId, command.RoleId, command.CountryId);

        if (!_userRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}