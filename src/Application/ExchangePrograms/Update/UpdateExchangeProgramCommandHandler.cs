using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.ExchangePrograms;
using Domain.ExchangePrograms.ValueObjects;

namespace Application.ExchangePrograms.Update;
internal sealed class UpdateExchangeProgramCommandHandler : ICommandHandler<UpdateExchangeProgramCommand>
{
    private readonly IExchangeProgramRepository _exchangeProgramRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateExchangeProgramCommandHandler(IExchangeProgramRepository exchangeProgramRepository, IUnitOfWork unitOfWork)
    {
        _exchangeProgramRepository = exchangeProgramRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateExchangeProgramCommand command, CancellationToken cancellationToken = default)
    {
        var id = new ExchangeProgramId(command.Id);

        var exchangeProgram = await _exchangeProgramRepository.GetById(id);

        if (exchangeProgram is null)
        {
            return ExchangeProgramErrors.NotFound(command.Id);
        }

        var name = new ExchangeProgramName(command.Name);
        var description = new ExchangeProgramDescription(command.Description);
        var limitApplicationDate = new ExchangeProgramLimitApplicationDate(command.LimitApplicationDate);
        var startDate = new ExchangeProgramStartDate(command.StartDate);
        var finishDate = new ExchangeProgramFinishDate(command.FinishDate);
        var applicationDocuments = new ExchangeProgramApplicationDocuments(command.ApplicationDocuments);
        var requiredDocuments = new ExchangeProgramRequiredDocuments(command.RequiredDocuments);
        var images = new ExchangeProgramImages(command.Images);

        ExchangeProgram.Update(exchangeProgram, name, description, limitApplicationDate, startDate, finishDate, applicationDocuments, requiredDocuments, images, command.CountryId, command.StateId, command.StatusId);

        if (!_exchangeProgramRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
