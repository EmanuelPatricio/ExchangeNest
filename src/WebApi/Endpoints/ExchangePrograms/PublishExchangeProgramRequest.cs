namespace WebApi.Endpoints.ExchangePrograms;

public sealed record PublishExchangeProgramRequest(
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