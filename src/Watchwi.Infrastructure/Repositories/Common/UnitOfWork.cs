using Watchwi.Domain.Common.IRepositories;
using Watchwi.Infrastructure.Persistence;

namespace Watchwi.Infrastructure.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}