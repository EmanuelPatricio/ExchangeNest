using Domain.Abstractions;
using Domain.Applications.Events;
using Domain.Applications.ValueObjects;
using static Domain.Shared.Enums;

namespace Domain.Applications;
public sealed class Application : Entity<ApplicationId>
{
    private Application() { }

    private Application(
        ApplicationId id,
        int programId,
        int studentId,
        ApplicationReason reason,
        int statusId,
        List<ApplicationDocument> documents,
        DateTime createdOn,
        DateTime? lastModifiedOn)
        : base(id)
    {
        ProgramId = programId;
        StudentId = studentId;
        Reason = reason;
        StatusId = statusId;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
        Documents = documents;
    }

    public int ProgramId { get; private set; }
    public int StudentId { get; private set; }
    public ApplicationReason Reason { get; private set; }
    public int StatusId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }
    public ICollection<ApplicationDocument> Documents { get; } = new List<ApplicationDocument>();

    public static Application Create(
        ApplicationId id,
        int programId,
        int studentId,
        ApplicationReason reason,
        int statusId,
        List<ApplicationDocument> documents)
    {
        var application = new Application(id, programId, studentId, reason, statusId, documents, DateTime.Now, null);

        application.RaiseDomainEvent(new ApplicationCreatedDomainEvent(application.Id));

        return application;
    }

    public static void Update(
        Application application,
        ApplicationReason reason,
        int statusId)
    {
        application.Reason = reason;
        application.StatusId = statusId;
        application.LastModifiedOn = DateTime.Now;

        application.RaiseDomainEvent(new ApplicationUpdatedDomainEvent(application.Id));
    }

    public static void Cancel(Application application, ApplicationReason reason)
    {
        application.Reason = reason;
        application.StatusId = (int)Statuses.Cancelled;
        application.LastModifiedOn = DateTime.Now;

        application.RaiseDomainEvent(new ApplicationUpdatedDomainEvent(application.Id));
    }

    public static void Close(Application application, ApplicationReason reason)
    {
        application.Reason = reason;
        application.StatusId = (int)Statuses.Closed;
        application.LastModifiedOn = DateTime.Now;

        application.RaiseDomainEvent(new ApplicationUpdatedDomainEvent(application.Id));
    }
}
