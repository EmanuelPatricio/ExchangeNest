using Domain.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class ConfigurationRepository : Repository, IConfigurationRepository
{
    public ConfigurationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public bool DoesDatabaseHasChanges()
    {
        return HasChanges();
    }

    public bool DoesExistsMoreThanOneOrNoneConfiguration()
    {
        return _db.Configurations.ToList().Count() > 1 || !_db.Configurations.Any();
    }

    public async Task<Domain.Configurations.Configuration?> Get()
    {
        return await _db.Configurations.FirstOrDefaultAsync(x => x.Id == new ConfigurationId(1));
    }

    public async Task KeepJustOneConfiguration()
    {
        var configuration = await Get();

        if (configuration is null)
        {
            _db.Configurations.Add(new(new(1), string.Empty, string.Empty, string.Empty));
        }

        var otherConfigurations = _db.Configurations.Where(x => x.Id != new ConfigurationId(1)).AsEnumerable();

        _db.Configurations.RemoveRange(otherConfigurations);

        await _db.SaveChangesAsync();
    }
}
