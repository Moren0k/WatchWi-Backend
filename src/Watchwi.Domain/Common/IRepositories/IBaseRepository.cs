namespace Watchwi.Domain.Common.IRepositories;

public interface IBaseRepository<T> where T : class
{
    // WRITE
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    
    // READ
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
}