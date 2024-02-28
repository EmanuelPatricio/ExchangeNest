using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.ExchangePrograms;

namespace Application.ExchangePrograms.Close;
internal sealed class CloseExchangeProgramCommandHandler : ICommandHandler<CloseExchangeProgramCommand>
{
    private readonly IExchangeProgramRepository _exchangeProgramRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseExchangeProgramCommandHandler(IExchangeProgramRepository exchangeProgramRepository, IUnitOfWork unitOfWork)
    {
        _exchangeProgramRepository = exchangeProgramRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CloseExchangeProgramCommand command, CancellationToken cancellationToken = default)
    {
        var exchangeProgram = await _exchangeProgramRepository.GetById(new ExchangeProgramId(command.Id));

        if (exchangeProgram is null)
        {
            return ExchangeProgramErrors.NotFound(command.Id);
        }

        ExchangeProgram.Close(exchangeProgram);

        if (!_exchangeProgramRepository.DoesDatabaseHasChanges())
        {
            return Error.NoChangesDetected;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
