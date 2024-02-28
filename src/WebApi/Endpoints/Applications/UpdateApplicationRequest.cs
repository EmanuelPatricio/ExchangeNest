using Application.Applications;

namespace WebApi.Endpoints.Applications;
public sealed record UpdateApplicationRequest(
        int Id,
        string Reason,
        int StatusId,
        List<ApplicationDocumentValues> ApplicationDocuments,
        List<ApplicationDocumentValues> RequiredDocuments);