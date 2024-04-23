using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Users.Shared;
using Domain.Abstractions;
using Domain.Organizations;
using Domain.Organizations.ValueObjects;
using Domain.Shared;
using Domain.Users;
using static Domain.Shared.Enums;

namespace Application.Organizations.Register;
internal sealed class RegisterOrganizationCommandHandler : ICommandHandler<RegisterOrganizationCommand>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;

    public RegisterOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    public async Task<Result> Handle(RegisterOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        var email = emailResult.Value;

        if (!await _organizationRepository.IsEmailUniqueAsync(email))
        {
            return UserErrors.EmailNotUnique;
        }

        var id = new OrganizationId(command.Id);
        var name = new OrganizationName(command.Name);
        var description = new OrganizationDescription(command.Description);
        var phoneNumber = new OrganizationPhoneNumber(command.PhoneNumber);
        var address = new OrganizationAddress(command.Address);
        var imageUrl = new OrganizationImageUrl(command.ImageUrl);

        var organization = Organization.Create(id, name, description, email, phoneNumber, address, imageUrl, command.OrganizationTypeId, command.StatusId);

        _organizationRepository.Create(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = EncodePassword.EncodeToBase64(command.Id.ToString());

        var emailMessage = await _emailSender.GetEmailHtmlFileData(EmailHtmlFile.RegisterOrganization);

        if (emailMessage.IsFailure)
        {
            return Email.NotSended;
        }

        var message = emailMessage.Value.Replace("[Organization name]", $"{command.Name}");
        message = message.Replace("[url]", $"{command.Url}token={token}");

        var result = await _emailSender.SendEmailAsync(new EmailRequest(
            To: command.Email,
            Subject: $"Welcome to {command.Name} in our system!",
            Message: message));

        if (result.IsFailure)
        {
            return Email.NotSended;
        }

        return Result.Success();
    }
}
