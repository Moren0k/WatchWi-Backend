using Watchwi.Application.DTOs;
using Watchwi.Application.IProviders.Cloudinary;
using Watchwi.Domain.Common;
using Watchwi.Domain.Common.IRepositories;
using Watchwi.Domain.Entities.Images;
using Watchwi.Domain.Entities.Medias;
using Watchwi.Domain.Entities.Users;

namespace Watchwi.Application.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly IImageRepository _imageRepository;
    private readonly ICloudinaryProvider _cloudinaryProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        IUserRepository userRepository,
        IMediaRepository mediaRepository,
        IImageRepository imageRepository,
        ICloudinaryProvider cloudinaryProvider,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mediaRepository = mediaRepository;
        _imageRepository = imageRepository;
        _cloudinaryProvider = cloudinaryProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<UserDto>> GetProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdWithProfileImageAsync(userId);
        if (user == null)
            return Result<UserDto>.Failure("User not found");
        
        return Result<UserDto>.Success(new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Role.ToString(),
            user.Status,
            user.Plan.ToString(),
            user.ProfileImage?.Url
        ));
    }

    public async Task<Result<UserDto>> UpdateProfileAsync(Guid userId, UpdateUserRequestDto request)
    {
        var user = await _userRepository.GetByIdWithProfileImageAsync(userId);
        if (user == null)
            return Result<UserDto>.Failure("User not found");

        // Check if username is changing and uniqueness
        if (request.Username != user.Username)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            // If existingUser is found and it's NOT the current user (though ID comparison is safer)
            if (existingUser != null && existingUser.Id != userId)
                return Result<UserDto>.Failure($"Username '{request.Username}' is already taken.");
            
            try 
            {
                user.UpdateProfile(request.Username);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure(ex.Message);
            }
        }
        
        return Result<UserDto>.Success(new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Role.ToString(),
            user.Status,
            user.Plan.ToString(),
            user.ProfileImage?.Url
        ));
    }

    public async Task<Result<UserDto>> UploadProfileImageAsync(Guid userId, UploadProfileImageRequestDto request)
    {
        var user = await _userRepository.GetByIdWithProfileImageAsync(userId);

        if (user == null)
             return Result<UserDto>.Failure("User not found");

        if (request.File.Length == 0)
            return Result<UserDto>.Failure("File is empty");

        try 
        {
            var uploadResponse = await _cloudinaryProvider.UploadAsync(new UploadMediaRequest
            {
                FileName = request.File.FileName,
                FileStream = request.File.Content,
                Folder = "ImgProfiles"
            });

            if (user.ProfileImage != null)
            {
                // Option A: Reuse existing entity
                user.ProfileImage.Update(uploadResponse.PublicId, uploadResponse.Url);
            }
            else
            {
                // Option A: Create new entity and ensure ADDED state
                var newImage = new Image(uploadResponse.PublicId, uploadResponse.Url);
                _imageRepository.Add(newImage);
                user.SetProfileImage(newImage);
            }
            
            await _unitOfWork.SaveChangesAsync();

            return Result<UserDto>.Success(new UserDto(
                user.Id,
                user.Username,
                user.Email,
                user.Role.ToString(),
                user.Status,
                user.Plan.ToString(),
                user.ProfileImage?.Url
            ));
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
    }

    public async Task<Result> AddFavoriteAsync(Guid userId, Guid mediaId)
    {
        var user = await _userRepository.GetByIdWithFavoritesLiteAsync(userId);
        if (user == null) return Result.Failure("User not found");

        var media = await _mediaRepository.GetByIdAsync(mediaId);
        if (media == null) return Result.Failure("Media not found");

        user.MarkAsFavorite(media);
        
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RemoveFavoriteAsync(Guid userId, Guid mediaId)
    {
         var user = await _userRepository.GetByIdWithFavoritesLiteAsync(userId);
         if (user == null) return Result.Failure("User not found");
         
         // Using a stub media object for RemoveAsFavorite since we only need the ID 
         // and the domain method handles finding it in the collection.
         // Alternatively, we could fetch the media, but it's more efficient to just 
         // pass what's needed or let the domain handling it as we implemented.
         // Given the domain implementation: existing = FavoriteMedias.FirstOrDefault(m => m.Id == media.Id)
         // we can just pass a media with the Id.
         var media = user.FavoriteMedias.FirstOrDefault(m => m.Id == mediaId);
         
         if (media != null)
         {
             user.RemoveAsFavorite(media);
             await _unitOfWork.SaveChangesAsync();
         }
         
         return Result.Success();
    }

    public async Task<Result<IReadOnlyList<FavoriteMediaDto>>> GetFavoritesAsync(Guid userId)
    {
         var user = await _userRepository.GetByIdWithFavoritesAsync(userId);
         if (user == null) return Result<IReadOnlyList<FavoriteMediaDto>>.Failure("User not found");

         var dtos = user.FavoriteMedias.Select(m => new FavoriteMediaDto(
             m.Id,
             m.Title,
             m.Poster?.Url,
             m.Description
         )).ToList();

         return Result<IReadOnlyList<FavoriteMediaDto>>.Success(dtos);
    }
}
