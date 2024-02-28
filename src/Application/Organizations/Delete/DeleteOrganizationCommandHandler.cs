using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Organizations;

namespace Application.Organizations.Delete;
internal sealed class DeleteOrganizationCommandHandler : ICommandHandler<DeleteOrganizationCommand>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var organization = await _organizationRepository.GetById(new OrganizationId(command.Id));

        if (organization is null)
        {
            return OrganizationErrors.NotFound(command.Id);
        }

        Organization.Deactivate(organization);

        if (!_organizationRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
