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

namespace Application.Applications.Publish;

internal sealed class PublishApplicationCommandHandler : ICommandHandler<PublishApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly IExchangeProgramRepository _exchangeProgramRepository;

    public PublishApplicationCommandHandler(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork, IEmailSender emailSender, IUserRepository userRepository, IExchangeProgramRepository exchangeProgramRepository)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _userRepository = userRepository;
        _exchangeProgramRepository = exchangeProgramRepository;
    }

    public async Task<Result> Handle(PublishApplicationCommand command, CancellationToken cancellationToken = default)
    {
        var applications = await _applicationRepository.GetAll();

        var userHasAlreadyApplied = applications.Any(x => x.StudentId == command.StudentId && x.StatusId != (int)Enums.Statuses.Cancelled && x.ProgramId == command.ProgramId);

        if (userHasAlreadyApplied)
        {
            return new Error("Application", "You have already made an application to this program.");
        }

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

        var application = Domain.Applications.Application.Create(id, command.ProgramId, command.StudentId, reason, command.StatusId, documents);

        _applicationRepository.Create(application);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var user = await _userRepository.GetById(new(command.StudentId));

        if (user is null)
        {
            return UserErrors.NotFound(command.StudentId);
        }

        var exchangeProgram = await _exchangeProgramRepository.GetById(new(command.ProgramId));

        if (exchangeProgram is null)
        {
            return ExchangeProgramErrors.NotFound(command.ProgramId);
        }

        var emailMessage = await _emailSender.GetEmailHtmlFileData(EmailHtmlFile.PublishApplication);

        if (emailMessage.IsFailure)
        {
            return Email.NotSended;
        }

        var message = emailMessage.Value.Replace("[User name]", user.FirstName.Value);
        message = message.Replace("[Exchange program name]", exchangeProgram.Name.Value);
        message = message.Replace("[url]", command.Url);

        await _emailSender.SendEmailAsync(new(
            To: user.Email.Value,
            Subject: "Your application for the exchange program has been successfully published!",
            Message: message));

        return Result.Success();
    }
}