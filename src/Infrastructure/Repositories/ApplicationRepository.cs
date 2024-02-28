using Domain.Applications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class ApplicationRepository : Repository<Domain.Applications.Application, Domain.Applications.ApplicationId>, IApplicationRepository
{
    public ApplicationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void Create(Domain.Applications.Application application)
    {
        Add(application);
    }

    public void Create(ApplicationDocument applicationDocument)
    {
        _db.ApplicationDocuments.Add(applicationDocument);
    }

    public void DeleteDocument(ApplicationDocumentId id)
    {
        var documentToRemove = _db.ApplicationDocuments.First(x => x.Id == id);
        _db.ApplicationDocuments.Remove(documentToRemove);
        _db.SaveChanges();
    }

    public bool DoesDatabaseHasChanges()
    {
        return HasChanges();
    }

    public async Task<List<Domain.Applications.Application>> GetAll()
    {
        return await _db.Applications.Include(x => x.Documents).ToListAsync();
    }

    public async Task<List<ApplicationDocument>> GetAllDocumentsByApplication(Domain.Applications.ApplicationId id)
    {
        return await _db.ApplicationDocuments.Where(x => x.ApplicationId == id).ToListAsync();
    }

    public async Task<Domain.Applications.Application?> GetById(Domain.Applications.ApplicationId id)
    {
        return await _db.Applications.Include(x => x.Documents).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ApplicationDocument?> GetDocumentById(ApplicationDocumentId id)
    {
        return await _db.ApplicationDocuments.FirstOrDefaultAsync(x => x.Id == id);
    }
}
