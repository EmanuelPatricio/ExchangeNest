using Domain.ExchangePrograms;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class ExchangeProgramRepository : Repository<ExchangeProgram, ExchangeProgramId>, IExchangeProgramRepository
{
    public ExchangeProgramRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void Create(ExchangeProgram exchangeProgram)
    {
        Add(exchangeProgram);
    }

    public async Task<List<ExchangeProgram>> GetAll()
    {
        return await _db.ExchangePrograms.ToListAsync();
    }

    public async Task<ExchangeProgram?> GetById(ExchangeProgramId id)
    {
        return await GetByIdAsync(id);
    }

    public bool DoesDatabaseHasChanges()
    {
        return HasChanges();
    }
}
