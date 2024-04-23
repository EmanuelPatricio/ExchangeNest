using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Shared;
using Domain.Abstractions;
using Domain.Configurations;

namespace Application.Configurations.Update;
internal sealed class UpdateConfigurationCommandHandler : ICommandHandler<UpdateConfigurationCommand>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateConfigurationCommandHandler(IConfigurationRepository configurationRepository, IUnitOfWork unitOfWork)
    {
        _configurationRepository = configurationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateConfigurationCommand command, CancellationToken cancellationToken = default)
    {
        if (_configurationRepository.DoesExistsMoreThanOneOrNoneConfiguration())
        {
            await _configurationRepository.KeepJustOneConfiguration();
        }

        var configuration = await _configurationRepository.Get();

        if (configuration is null)
        {
            return Error.NullValue;
        }

        var encryptedSenderPassword = EncodePassword.EncodeToBase64(command.SenderPassword);

        Configuration.Update(configuration, command.SenderMail, encryptedSenderPassword, command.BaseTemplate, command.ForgotPassword, command.RegisterOrganization, command.PublishApplication, command.UpdateApplication, command.RegisterUser, command.CompletedApplication, command.CancelledApplication);

        if (!_configurationRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}