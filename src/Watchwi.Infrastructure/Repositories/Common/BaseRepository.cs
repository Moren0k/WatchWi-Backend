using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Common.IRepositories;
using Watchwi.Infrastructure.Persistence;

namespace Watchwi.Infrastructure.Repositories.Common;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<T> DbSet;

    protected BaseRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    // WRITE
    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
    
    // READ
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
}