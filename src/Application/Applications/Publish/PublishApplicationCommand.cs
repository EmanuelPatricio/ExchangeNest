using Application.Abstractions.Messaging;

namespace Application.Applications.Publish;
public sealed record PublishApplicationCommand(
        int Id,
        int ProgramId,
        int StudentId,
        string Reason,
        int StatusId,
        List<ApplicationDocumentValues> ApplicationDocuments,
        List<ApplicationDocumentValues> RequiredDocuments) : ICommand;