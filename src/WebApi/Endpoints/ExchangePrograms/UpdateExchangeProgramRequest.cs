namespace WebApi.Endpoints.ExchangePrograms;

public sealed record UpdateExchangeProgramRequest(
    int Id,
    string Name,
    string Description,
    DateTime LimitApplicationDate,
    DateTime StartDate,
    DateTime FinishDate,
    string ApplicationDocuments,
    string RequiredDocuments,
    string ImagesUrl,
    int CountryId,
    int StateId,
    int StatusId);