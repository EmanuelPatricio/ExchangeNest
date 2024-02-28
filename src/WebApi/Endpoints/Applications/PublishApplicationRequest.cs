using Application.Applications;

namespace WebApi.Endpoints.Applications;

public sealed record PublishApplicationRequest(
        int ProgramId,
        int StudentId,
        string Reason,
        int StatusId,
        List<ApplicationDocumentValues> ApplicationDocuments,
        List<ApplicationDocumentValues> RequiredDocuments);
