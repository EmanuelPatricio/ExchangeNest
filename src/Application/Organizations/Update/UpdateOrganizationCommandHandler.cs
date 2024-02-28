using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Organizations;
using Domain.Organizations.ValueObjects;
using Domain.Shared;
using Domain.Users;

namespace Application.Organizations.Update;
internal sealed class UpdateOrganizationCommandHandler : ICommandHandler<UpdateOrganizationCommand>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        var id = new OrganizationId(command.Id);
        var email = emailResult.Value;

        if (!await _organizationRepository.IsEmailUniqueAsync(email, id))
        {
            return UserErrors.EmailNotUnique;
        }

        var organization = await _organizationRepository.GetById(id);

        if (organization is null)
        {
            return UserErrors.NotFound(command.Id);
        }

        var name = new OrganizationName(command.Name);
        var description = new OrganizationDescription(command.Description);
        var phoneNumber = new OrganizationPhoneNumber(command.PhoneNumber);
        var address = new OrganizationAddress(command.Address);
        var imageUrl = new OrganizationImageUrl(command.ImageUrl);

        Organization.Update(organization, name, description, email, phoneNumber, address, imageUrl, command.OrganizationTypeId, command.StatusId);

        if (!_organizationRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
