using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Applications;
using Domain.Applications.ValueObjects;
using Domain.Shared;
using Domain.Users;

namespace Application.Applications.Update;
internal sealed class UpdateApplicationCommandHandler : ICommandHandler<UpdateApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;

    public UpdateApplicationCommandHandler(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork, IEmailSender emailSender, IUserRepository userRepository)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(UpdateApplicationCommand command, CancellationToken cancellationToken = default)
    {
        var documentsIds = new List<int>() { command.NextDocumentId };

        var id = new Domain.Applications.ApplicationId(command.Id);

        var application = await _applicationRepository.GetById(id);

        if (application is null)
        {
            return ApplicationErrors.NotFound(command.Id);
        }

        var reason = new ApplicationReason(command.Reason);

        foreach (var applicationDocument in command.ApplicationDocuments)
        {
            var document = application.Documents.FirstOrDefault(x => x.Id.Value == applicationDocument.Id && x.ApplicationId == id && x.DocumentType == (int)Enums.DocumentTypes.Application);

            if (document is null)
            {
                var newId = documentsIds.Last();

                _applicationRepository.Create(ApplicationDocument.Create(
                    new(newId + 1), id, applicationDocument.Category, (int)Enums.DocumentTypes.Application, applicationDocument.Url, applicationDocument.StatusId, applicationDocument.Reason));

                documentsIds.Add(newId + 1);
                continue;
            }

            ApplicationDocument.Update(document, applicationDocument.Category, (int)Enums.DocumentTypes.Application, applicationDocument.Url, applicationDocument.StatusId, applicationDocument.Reason);
        }

        foreach (var requiredDocument in command.RequiredDocuments)
        {
            var document = application.Documents.FirstOrDefault(x => x.Id.Value == requiredDocument.Id && x.ApplicationId == id && x.DocumentType == (int)Enums.DocumentTypes.Required);

            if (document is null)
            {
                var newId = documentsIds.Last();

                _applicationRepository.Create(ApplicationDocument.Create(
                    new(newId + 1), id, requiredDocument.Category, (int)Enums.DocumentTypes.Required, requiredDocument.Url, requiredDocument.StatusId, requiredDocument.Reason));

                documentsIds.Add(newId + 1);
                continue;
            }

            ApplicationDocument.Update(document, requiredDocument.Category, (int)Enums.DocumentTypes.Required, requiredDocument.Url, requiredDocument.StatusId, requiredDocument.Reason);
        }

        Domain.Applications.Application.Update(application, reason, command.StatusId);

        if (!_applicationRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var user = await _userRepository.GetById(new(application.StudentId));

        if (user is null)
        {
            return UserErrors.NotFound(application.StudentId);
        }

        await _emailSender.SendEmailAsync(new(user.Email.Value, "Updated Application", "Your application has been updated."));

        return Result.Success();
    }
}
