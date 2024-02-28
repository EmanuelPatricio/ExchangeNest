using Domain.Shared;

namespace Domain.Organizations;
public interface IOrganizationRepository
{
    void Create(Organization organization);
    bool DoesDatabaseHasChanges();
    Task<List<Organization>> GetAll();
    Task<Organization?> GetById(OrganizationId id);
    Task<bool> IsEmailUniqueAsync(Email email, OrganizationId? organizationId = null);
}
