namespace Domain.Applications;
public interface IApplicationRepository
{
    void Create(Application application);
    void Create(ApplicationDocument application);
    Task<Application?> GetById(ApplicationId id);
    void DeleteDocument(ApplicationDocumentId id);
    bool DoesDatabaseHasChanges();
    Task<List<Application>> GetAll();
    Task<ApplicationDocument?> GetDocumentById(ApplicationDocumentId id);
    Task<List<ApplicationDocument>> GetAllDocumentsByApplication(ApplicationId id);
}
