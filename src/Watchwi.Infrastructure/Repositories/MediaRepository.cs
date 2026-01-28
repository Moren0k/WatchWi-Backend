using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Entities.Medias;
using Watchwi.Infrastructure.Persistence;
using Watchwi.Infrastructure.Repositories.Common;

namespace Watchwi.Infrastructure.Repositories;

public sealed class MediaRepository : BaseRepository<Media>, IMediaRepository
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
        return await DbSet
            .Where(m => m.MediaType == mediaType)
            .Include(m => m.Poster)
            .ToListAsync();
    }

    public async Task<Media?> GetFeaturedAsync()
    {
        return await DbSet
            .Include(m => m.Poster)
            .FirstOrDefaultAsync(m => m.IsFeatured);
    }

    public async Task<IReadOnlyList<Media>> GetAllWithPosterAsync()
    {
        return await DbSet
            .Include(m => m.Poster)
            .ToListAsync();
    }

    public async Task<Media?> GetByIdWithDetailsAsync(Guid id)
    {
        return await DbSet
            .Include(m => m.Poster)
            .Include(m => m.Categories)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}