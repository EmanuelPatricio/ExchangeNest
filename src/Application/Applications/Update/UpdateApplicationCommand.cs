using Application.Abstractions.Messaging;

namespace Application.Applications.Update;
public sealed record UpdateApplicationCommand(
        int Id,
        string Reason,
        int StatusId,
        List<ApplicationDocumentValues> ApplicationDocuments,
        List<ApplicationDocumentValues> RequiredDocuments,
        int NextDocumentId,
        string Url,
        int UserId) : ICommand;