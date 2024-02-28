namespace Application.Applications.Get;
public sealed record GetApplicationResponse(
    int Id,
    int ProgramId,
    int StudentId,
    string Reason,
    int StatusId,
    Dictionary<int, string> ApplicationDocuments,
    Dictionary<int, string> RequiredDocuments);