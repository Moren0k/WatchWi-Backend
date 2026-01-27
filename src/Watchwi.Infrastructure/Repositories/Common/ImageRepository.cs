using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Entities.Images;
using Watchwi.Domain.Entities.Users;
using Watchwi.Infrastructure.Persistence;

namespace Watchwi.Infrastructure.Repositories.Common;

public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    protected ImageRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Image?> GetByPublicIdAsync(string publicId)
    {
        return await DbSet.FirstOrDefaultAsync(i => i.PublicId == publicId);
    }
}