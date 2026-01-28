namespace Watchwi.Application.Services.MediaService;

public interface IMediaService
{
    Task<Guid> CreateWithPosterAsync(CreateMediaWithPosterCommand command);
    Task<IReadOnlyList<MediaListItemResponse>> GetAllAsync();
    Task<MediaDetailResponse> GetByIdAsync(Guid id);
    Task MarkAsFeaturedAsync(Guid id);
    Task DeleteAsync(Guid id);
}