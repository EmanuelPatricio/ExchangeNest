using Application.Abstractions.Messaging;

namespace Application.ExchangePrograms.Publish;
public sealed record PublishExchangeProgramCommand(
        int Id,
        string Name,
        string Description,
        DateTime LimitApplicationDate,
        DateTime StartDate,
        DateTime FinishDate,
        string ApplicationDocuments,
        string RequiredDocuments,
        string Images,
        int OrganizationId,
        int CountryId,
        int StateId,
        int StatusId) : ICommand;