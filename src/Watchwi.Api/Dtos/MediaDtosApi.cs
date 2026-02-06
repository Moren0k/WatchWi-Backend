using Watchwi.Domain.Entities.Medias;

namespace Watchwi.Api.Dtos;

public sealed class CreateMediaRequest
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public MediaType MediaType { get; init; }
    public string MediaUrl { get; init; } = null!;
    public IFormFile Poster { get; init; } = null!;
    public IEnumerable<Guid> CategoryIds { get; init; } = new List<Guid>();
}