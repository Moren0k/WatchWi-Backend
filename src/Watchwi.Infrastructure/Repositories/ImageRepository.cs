using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Entities.Images;
using Watchwi.Infrastructure.Persistence;
using Watchwi.Infrastructure.Repositories.Common;

namespace Watchwi.Infrastructure.Repositories;

public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    public ImageRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Image?> GetByPublicIdAsync(string publicId)
    {
        return await DbSet.FirstOrDefaultAsync(i => i.PublicId == publicId);
    }
}