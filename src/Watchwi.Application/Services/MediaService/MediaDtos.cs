using Watchwi.Domain.Entities.Medias;

namespace Watchwi.Application.Services.MediaService;

public sealed record CreateMediaWithPosterCommand(
    string Title,
    string Description,
    MediaType MediaType,
    string MediaUrl,
    Stream PosterStream,
    string PosterFileName,
    IEnumerable<Guid> CategoryIds
);

public sealed record MediaListItemResponse(
    Guid Id,
    string Title,
    string Description,
    string PosterUrl,
    string MediaUrl,
    bool IsFeatured
);

public sealed record MediaDetailResponse(
    Guid Id,
    string Title,
    string Description,
    string MediaType,
    string MediaUrl,
    bool IsFeatured,
    string PosterUrl,
    IReadOnlyList<string> Categories
);