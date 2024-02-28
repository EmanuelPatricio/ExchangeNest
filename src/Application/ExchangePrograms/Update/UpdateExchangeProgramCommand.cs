using Application.Abstractions.Messaging;

namespace Application.ExchangePrograms.Update;
public sealed record UpdateExchangeProgramCommand(
        int Id,
        string Name,
        string Description,
        DateTime LimitApplicationDate,
        DateTime StartDate,
        DateTime FinishDate,
        string ApplicationDocuments,
        string RequiredDocuments,
        string Images,
        int CountryId,
        int StateId,
        int StatusId) : ICommand;