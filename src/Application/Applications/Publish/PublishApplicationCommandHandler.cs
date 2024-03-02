using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Applications;
using Domain.Applications.ValueObjects;
using Domain.Shared;
using Domain.Users;

namespace Application.Applications.Publish;

internal sealed class PublishApplicationCommandHandler : ICommandHandler<PublishApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;

    public PublishApplicationCommandHandler(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork, IEmailSender emailSender, IUserRepository userRepository)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(PublishApplicationCommand command, CancellationToken cancellationToken = default)
    {
        var id = new Domain.Applications.ApplicationId(command.Id);
        var reason = new ApplicationReason(command.Reason);

        var documents = new List<ApplicationDocument>();

        foreach (var applicationDocument in command.ApplicationDocuments)
        {
            documents.Add(ApplicationDocument.Create(new(applicationDocument.Id), id, applicationDocument.Category, (int)Enums.DocumentTypes.Application, applicationDocument.Url, applicationDocument.StatusId, applicationDocument.Reason));
        }

        foreach (var requiredDocument in command.RequiredDocuments)
        {
            documents.Add(ApplicationDocument.Create(new(requiredDocument.Id), id, requiredDocument.Category, (int)Enums.DocumentTypes.Required, requiredDocument.Url, requiredDocument.StatusId, requiredDocument.Reason));
        }

        var exchangeProgram = Domain.Applications.Application.Create(id, command.ProgramId, command.StudentId, reason, command.StatusId, documents);

        _applicationRepository.Create(exchangeProgram);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var user = await _userRepository.GetById(new(command.StudentId));

        if (user is null)
        {
            return UserErrors.NotFound(command.StudentId);
        }

        await _emailSender.SendEmailAsync(new(user.Email.Value, "New Application", "You have successfully applied to a program."));

        return Result.Success();
    }
}