using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Entities.Users;
using Watchwi.Infrastructure.Persistence;
using Watchwi.Infrastructure.Repositories.Common;

namespace Watchwi.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await  DbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}