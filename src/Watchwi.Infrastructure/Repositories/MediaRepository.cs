using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Entities.Medias;
using Watchwi.Infrastructure.Persistence;
using Watchwi.Infrastructure.Repositories.Common;

namespace Watchwi.Infrastructure.Repositories;

public class MediaRepository : BaseRepository<Media>, IMediaRepository
{
    public MediaRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Media?> GetByTitleAsync(string title)
    {
        return await DbSet.FirstOrDefaultAsync(m => m.Title == title);
    }

    public async Task<IReadOnlyList<Media>> GetByTypeAsync(MediaType mediaType)
    {
        return await DbSet.Where(c => c.MediaType == mediaType).ToListAsync();
    }

    public async Task<Media?> GetFeaturedAsync()
    {
        return await DbSet.FirstOrDefaultAsync(m => m.IsFeatured);
    }
}