using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.ExchangePrograms;
using Domain.ExchangePrograms.ValueObjects;

namespace Application.ExchangePrograms.Publish;
internal sealed class PublishExchangeProgramCommandHandler : ICommandHandler<PublishExchangeProgramCommand>
{
    private readonly IExchangeProgramRepository _exchangeProgramRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishExchangeProgramCommandHandler(IExchangeProgramRepository exchangeProgramRepository, IUnitOfWork unitOfWork)
    {
        _exchangeProgramRepository = exchangeProgramRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PublishExchangeProgramCommand command, CancellationToken cancellationToken = default)
    {
        var id = new ExchangeProgramId(command.Id);
        var name = new ExchangeProgramName(command.Name);
        var description = new ExchangeProgramDescription(command.Description);
        var limitApplicationDate = new ExchangeProgramLimitApplicationDate(command.LimitApplicationDate);
        var startDate = new ExchangeProgramStartDate(command.StartDate);
        var finishDate = new ExchangeProgramFinishDate(command.FinishDate);
        var applicationDocuments = new ExchangeProgramApplicationDocuments(command.ApplicationDocuments);
        var requiredDocuments = new ExchangeProgramRequiredDocuments(command.RequiredDocuments);
        var images = new ExchangeProgramImages(command.Images);

        var exchangeProgram = ExchangeProgram.Create(id, name, description, limitApplicationDate, startDate, finishDate, applicationDocuments, requiredDocuments, images, command.OrganizationId, command.CountryId, command.StateId, command.StatusId);

        _exchangeProgramRepository.Create(exchangeProgram);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
