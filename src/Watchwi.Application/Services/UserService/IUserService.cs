using Watchwi.Domain.Common;
using Watchwi.Application.DTOs;

namespace Watchwi.Application.Services.UserService;

public interface IUserService
{
    Task<Result<UserDto>> UpdateProfileAsync(Guid userId, UpdateUserRequestDto request);
    Task<Result<UserDto>> UploadProfileImageAsync(Guid userId, UploadProfileImageRequestDto request);
    Task<Result> AddFavoriteAsync(Guid userId, Guid mediaId);
    Task<Result> RemoveFavoriteAsync(Guid userId, Guid mediaId);
    Task<Result<IReadOnlyList<FavoriteMediaDto>>> GetFavoritesAsync(Guid userId);
}
