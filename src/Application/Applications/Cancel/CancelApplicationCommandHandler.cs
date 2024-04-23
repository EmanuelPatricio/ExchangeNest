using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Applications;
using Domain.Applications.ValueObjects;
using Domain.ExchangePrograms;
using Domain.Shared;
using Domain.Users;

namespace Application.Applications.Cancel;
internal sealed class CancelApplicationCommandHandler : ICommandHandler<CancelApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly IExchangeProgramRepository _exchangeProgramRepository;

    public CancelApplicationCommandHandler(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork, IEmailSender emailSender, IUserRepository userRepository, IExchangeProgramRepository exchangeProgramRepository)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _userRepository = userRepository;
        _exchangeProgramRepository = exchangeProgramRepository;
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

        var user = await _userRepository.GetById(new(application.StudentId));

        if (user is null)
        {
            return UserErrors.NotFound(application.StudentId);
        }

        var exchangeProgram = await _exchangeProgramRepository.GetById(new(application.ProgramId));

        if (exchangeProgram is null)
        {
            return ExchangeProgramErrors.NotFound(application.ProgramId);
        }

        var emailMessage = await _emailSender.GetEmailHtmlFileData(Enums.EmailHtmlFile.CancelledApplication);

        if (emailMessage.IsFailure)
        {
            return Email.NotSended;
        }

        var message = emailMessage.Value.Replace("[User name]", user.FirstName.Value);
        message = message.Replace("[Exchange program name]", exchangeProgram.Name.Value);

        await _emailSender.SendEmailAsync(new(
            To: user.Email.Value,
            Subject: "Confirmation of cancelled application for the exchange program",
            Message: message));

        return Result.Success();
    }
}