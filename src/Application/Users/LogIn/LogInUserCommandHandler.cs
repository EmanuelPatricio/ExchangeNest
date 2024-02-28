using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;
using Domain.Users.ValueObjects;
using static Domain.Shared.Enums;

namespace Application.Users.LogIn;
internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, LogInUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LogInUserCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<LogInUserResponse>> Handle(
        LogInUserCommand request, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<LogInUserResponse>(emailResult.Error);
        }

        var email = emailResult.Value;

        if (await _userRepository.IsEmailUniqueAsync(email))
        {
            return Result.Failure<LogInUserResponse>(UserErrors.NotFoundEmail);
        }

        var user = await _userRepository.LoginAsync(email, new UserPassword(request.Password), cancellationToken);

        if (user is null)
        {
            return Result.Failure<LogInUserResponse>(UserErrors.IncorrectPassword);
        }

        var result = _jwtService.GetAccessToken(user.Id.Value, user.RoleId);

        if (result.IsFailure)
        {
            return Result.Failure<LogInUserResponse>(UserErrors.JwtUnexpectedError);
        }

        return Result.Success(new LogInUserResponse(user.Id.Value,
                                     user.FirstName.Value,
                                     user.LastName.Value,
                                     user.NIC.Value,
                                     user.Email.Value,
                                     user.BirthDate.Value,
                                     ((Roles)user.RoleId).ToString(),
                                     user.StatusId,
                                     user.OrganizationId,
                                     user.CountryId,
                                     result.Value));
    }
}
