using Domain.Organizations;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class OrganizationRepository : Repository<Organization, OrganizationId>, IOrganizationRepository
{
    public OrganizationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void Create(Organization organization)
    {
        Add(organization);
    }

    public async Task<List<Organization>> GetAll()
    {
        return await _db.Organizations.ToListAsync();
    }

    public async Task<Organization?> GetById(OrganizationId id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, OrganizationId? organizationId = null)
    {
        var organization = await _db.Organizations.FirstOrDefaultAsync(u => u.Email == email);

        if (organization is not null && organizationId is not null)
        {
            return organization.Id == organizationId;
        }

        return organization is null;
    }

    public bool DoesDatabaseHasChanges()
    {
        return HasChanges();
    }
}
