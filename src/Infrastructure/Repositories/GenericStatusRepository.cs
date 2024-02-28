using Domain.GenericStatuses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class GenericStatusRepository : IGenericStatusRepository
{
    private readonly ApplicationDbContext _db;
    public GenericStatusRepository(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<List<GenericStatus>> GetAll()
    {
        return await _db.GenericStatuses.ToListAsync();
    }
}
