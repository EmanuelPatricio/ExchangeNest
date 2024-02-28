using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Applications;
using Domain.Applications.ValueObjects;

namespace Application.Applications.Cancel;
internal sealed class CancelApplicationCommandHandler : ICommandHandler<CancelApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelApplicationCommandHandler(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelApplicationCommand command, CancellationToken cancellationToken = default)
    {
        var id = new Domain.Applications.ApplicationId(command.Id);

        var application = await _applicationRepository.GetById(id);

        if (application is null)
        {
            return ApplicationErrors.NotFound(command.Id);
        }

        var reason = new ApplicationReason(command.Reason);

        Domain.Applications.Application.Cancel(application, reason);

        if (!_applicationRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}