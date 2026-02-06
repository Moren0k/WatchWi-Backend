using Watchwi.Application.DTOs;

namespace Watchwi.Application.Services.UserService;

public record UpdateUserRequestDto(string Username);

public record UploadProfileImageRequestDto(FileUploadRequest File);

public record FavoriteMediaDto(
    Guid Id,
    string Title,
    string? PosterUrl,
    string Description
);
