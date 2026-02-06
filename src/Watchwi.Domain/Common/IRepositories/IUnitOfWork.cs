namespace Watchwi.Domain.Common.IRepositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
}