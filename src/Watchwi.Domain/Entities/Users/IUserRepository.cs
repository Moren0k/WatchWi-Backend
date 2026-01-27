using Watchwi.Domain.Common.IRepositories;

namespace Watchwi.Domain.Entities.Users;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
}