using Watchwi.Application.IProviders.Cloudinary;
using Watchwi.Domain.Common.IRepositories;
using Watchwi.Domain.Entities.Categories;
using Watchwi.Domain.Entities.Images;
using Watchwi.Domain.Entities.Medias;

namespace Watchwi.Application.Services.MediaService;

public sealed class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly IImageRepository _imageRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICloudinaryProvider _cloudinaryProvider;
    private readonly IUnitOfWork _unitOfWork;

    public MediaService(
        IMediaRepository mediaRepository,
        IImageRepository imageRepository,
        ICategoryRepository categoryRepository,
        ICloudinaryProvider cloudinaryProvider,
        IUnitOfWork unitOfWork)
    {
        _mediaRepository = mediaRepository;
        _imageRepository = imageRepository;
        _categoryRepository = categoryRepository;
        _cloudinaryProvider = cloudinaryProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateWithPosterAsync(CreateMediaWithPosterCommand command)
    {
        var uploadResult = await _cloudinaryProvider.UploadAsync(new UploadMediaRequest
        {
            FileName = command.PosterFileName,
            FileStream = command.PosterStream,
            Folder = "posters"
        });

        try
        {
            var image = new Image(uploadResult.PublicId, uploadResult.Url);
            _imageRepository.Add(image);

            var media = new Media(
                command.Title,
                command.Description,
                command.MediaType,
                image,
                command.MediaUrl
            );

            foreach (var categoryId in command.CategoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                media.AddCategory(category);
            }

            _mediaRepository.Add(media);
            await _unitOfWork.SaveChangesAsync();

            return media.Id;
        }
        catch
        {
            await _cloudinaryProvider.DeleteAsync(uploadResult.PublicId);
            throw;
        }
    }

    public async Task<IReadOnlyList<MediaListItemResponse>> GetAllAsync()
    {
        var medias = await _mediaRepository.GetAllWithPosterAsync();

        return medias.Select(m => new MediaListItemResponse(
            m.Id,
            m.Title,
            m.Description,
            m.Poster!.Url,
            m.MediaUrl,
            m.IsFeatured
        )).ToList();
    }

    public async Task<MediaDetailResponse> GetByIdAsync(Guid id)
    {
        var media = await _mediaRepository.GetByIdWithDetailsAsync(id)
                    ?? throw new InvalidOperationException("Media not found.");

        return new MediaDetailResponse(
            media.Id,
            media.Title,
            media.Description,
            media.MediaType.ToString(),
            media.MediaUrl,
            media.IsFeatured,
            media.Poster!.Url,
            media.Categories.Select(c => c.Name).ToList()
        );
    }

    public async Task MarkAsFeaturedAsync(Guid id)
    {
        var media = await _mediaRepository.GetByIdAsync(id)
                    ?? throw new InvalidOperationException("Media not found.");

        media.MarkAsFeatured();
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var media = await _mediaRepository.GetByIdAsync(id)
                    ?? throw new InvalidOperationException("Media not found.");

        _mediaRepository.Remove(media);
        await _unitOfWork.SaveChangesAsync();
    }
}