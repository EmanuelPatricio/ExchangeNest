namespace Domain.Configurations;
public interface IConfigurationRepository
{
    bool DoesDatabaseHasChanges();
    bool DoesExistsMoreThanOneOrNoneConfiguration();
    Task KeepJustOneConfiguration();
    Task<Configuration?> Get();
}
