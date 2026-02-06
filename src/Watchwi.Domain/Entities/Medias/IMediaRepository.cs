using Watchwi.Domain.Common.IRepositories;

namespace Watchwi.Domain.Entities.Medias;

public interface IMediaRepository : IBaseRepository<Media>
{
    Task<Media?> GetByTitleAsync(string title);
    Task<IReadOnlyList<Media>> GetByTypeAsync(MediaType mediaType);
    Task<Media?> GetFeaturedAsync();
    Task<IReadOnlyList<Media>> GetAllWithPosterAsync();
    Task<Media?> GetByIdWithDetailsAsync(Guid id);
}