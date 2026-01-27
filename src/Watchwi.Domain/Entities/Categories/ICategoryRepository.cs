using Watchwi.Domain.Common.IRepositories;

namespace Watchwi.Domain.Entities.Categories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
}