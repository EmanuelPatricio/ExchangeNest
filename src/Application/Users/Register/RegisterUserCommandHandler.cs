using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Shared;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Users.Create;
internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        var email = emailResult.Value;

        if (!await _userRepository.IsEmailUniqueAsync(email))
        {
            return Result.Failure(UserErrors.EmailNotUnique);
        }

        var decodedToken = string.Empty;
        if (!string.IsNullOrWhiteSpace(command.Token))
        {
            decodedToken = EncodePassword.DecodeFrom64(command.Token);
        }

        var id = new UserId(command.Id);
        var firstName = new UserFirstName(command.FirstName);
        var lastName = new UserLastName(command.LastName);
        var nic = new UserNIC(command.Nic);
        var password = new UserPassword(EncodePassword.EncodeToBase64(command.Password));
        var birthDate = new UserBirthDate(command.BirthDate);
        var imageUrl = new UserImageUrl(command.ImageUrl);

        var user = User.Create(id, firstName, lastName, nic, email, password, birthDate, imageUrl, command.RoleId, command.StatusId, string.IsNullOrWhiteSpace(decodedToken) ? command.OrganizationId : int.Parse(decodedToken), command.CountryId);

        _userRepository.Create(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
