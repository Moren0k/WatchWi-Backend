namespace Watchwi.Domain.Entities;

public interface IUnitOfWork : IDisposable
{
    public Task<int> SaveChangesAsync();
}