namespace Application.Applications.Get;
public sealed record GetApplicationResponse(
    int Id,
    int ProgramId,
    int StudentId,
    string Reason,
    int StatusId,
    List<ApplicationDocumentValues> ApplicationDocuments,
    List<ApplicationDocumentValues> RequiredDocuments);