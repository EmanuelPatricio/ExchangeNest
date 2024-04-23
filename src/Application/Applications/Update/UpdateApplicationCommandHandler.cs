using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Applications;
using Domain.Applications.ValueObjects;
using Domain.ExchangePrograms;
using Domain.Shared;
using Domain.Users;
using static Domain.Shared.Enums;

namespace Application.Applications.Update;
internal sealed class UpdateApplicationCommandHandler : ICommandHandler<UpdateApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly IExchangeProgramRepository _exchangeProgramRepository;

    public UpdateApplicationCommandHandler(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork, IEmailSender emailSender, IUserRepository userRepository, IExchangeProgramRepository exchangeProgramRepository)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _userRepository = userRepository;
        _exchangeProgramRepository = exchangeProgramRepository;
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
                var newId = documentsIds.Max() + 1;

                _applicationRepository.Create(ApplicationDocument.Create(
                    new(newId), id, applicationDocument.Category, (int)Enums.DocumentTypes.Application, applicationDocument.Url, applicationDocument.StatusId, applicationDocument.Reason));

                documentsIds.Add(newId);
                continue;
            }

            ApplicationDocument.Update(document, applicationDocument.Category, (int)Enums.DocumentTypes.Application, applicationDocument.Url, applicationDocument.StatusId, applicationDocument.Reason);
        }

        foreach (var requiredDocument in command.RequiredDocuments)
        {
            var document = application.Documents.FirstOrDefault(x => x.Id.Value == requiredDocument.Id && x.ApplicationId == id && x.DocumentType == (int)Enums.DocumentTypes.Required);

            if (document is null)
            {
                var newId = documentsIds.Max() + 1;

                _applicationRepository.Create(ApplicationDocument.Create(
                    new(newId), id, requiredDocument.Category, (int)Enums.DocumentTypes.Required, requiredDocument.Url, requiredDocument.StatusId, requiredDocument.Reason));

                documentsIds.Add(newId);
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

        if (user.Id.Value == command.UserId)
        {
            return Result.Success();
        }

        var exchangeProgram = await _exchangeProgramRepository.GetById(new(application.ProgramId));

        if (exchangeProgram is null)
        {
            return ExchangeProgramErrors.NotFound(application.ProgramId);
        }

        var emailMessage = await _emailSender.GetEmailHtmlFileData(EmailHtmlFile.UpdateApplication);

        if (emailMessage.IsFailure)
        {
            return Email.NotSended;
        }

        var message = emailMessage.Value.Replace("[User name]", user.FirstName.Value);
        message = message.Replace("[Exchange program name]", exchangeProgram.Name.Value);
        message = message.Replace("[url]", command.Url);

        await _emailSender.SendEmailAsync(new(
            To: user.Email.Value,
            Subject: "Application update for the exchange program",
            Message: message));

        return Result.Success();
    }
}
