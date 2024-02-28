namespace Application.ExchangePrograms.Get;
public sealed record GetExchangeProgramResponse(
    int Id,
    string Name,
    string Description,
    DateTime LimitApplicationDate,
    DateTime StartDate,
    DateTime FinishDate,
    string ApplicationDocuments,
    string RequiredDocuments,
    string ImagesUrl,
    int OrganizationId,
    int CountryId,
    int StateId,
    int StatusId);