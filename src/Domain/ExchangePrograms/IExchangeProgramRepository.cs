namespace Domain.ExchangePrograms;
public interface IExchangeProgramRepository
{
    void Create(ExchangeProgram user);
    Task<ExchangeProgram?> GetById(ExchangeProgramId id);
    bool DoesDatabaseHasChanges();
    Task<List<ExchangeProgram>> GetAll();
}
