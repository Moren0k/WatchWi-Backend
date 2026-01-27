using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Entities.Categories;
using Watchwi.Infrastructure.Persistence;
using Watchwi.Infrastructure.Repositories.Common;

namespace Watchwi.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Name == name);
    }
}