using Domain.Abstractions;

namespace Domain.Applications;
public sealed class ApplicationDocument : Entity<ApplicationDocumentId>
{
    private ApplicationDocument() { }

    private ApplicationDocument(
        ApplicationDocumentId id,
        ApplicationId applicationId,
        string documentCategory,
        int documentType,
        string documentUrl,
        DateTime createdOn,
        DateTime? lastModifiedOn)
        : base(id)
    {
        DocumentCategory = documentCategory;
        ApplicationId = applicationId;
        DocumentType = documentType;
        DocumentUrl = documentUrl;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    public string DocumentCategory { get; private set; }
    public int DocumentType { get; private set; }
    public string DocumentUrl { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }
    public ApplicationId ApplicationId { get; private set; }
    public Application Application { get; set; } = null!;

    public static ApplicationDocument Create(
        ApplicationDocumentId id,
        ApplicationId applicationId,
        string documentCategory,
        int documentType,
        string documentUrl)
    {
        var applicationDocument = new ApplicationDocument(id, applicationId, documentCategory, documentType, documentUrl, DateTime.Now, null);

        return applicationDocument;
    }

    public static void Update(
        ApplicationDocument applicationDocument,
        string documentCategory,
        int documentType,
        string documentUrl)
    {
        applicationDocument.DocumentCategory = documentCategory;
        applicationDocument.DocumentType = documentType;
        applicationDocument.DocumentUrl = documentUrl;
        applicationDocument.LastModifiedOn = DateTime.Now;
    }
}
