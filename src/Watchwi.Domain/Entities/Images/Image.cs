using Watchwi.Domain.Common;

namespace Watchwi.Domain.Entities.Images;

public class Image : BaseEntity
{
    public string PublicId { get; private set; } = null!;
    public string Url { get; private set; } = null!;

    protected Image()
    {
        // Required by EF Core
    }

    public Image(string publicId, string url)
    {
        SetPublicId(publicId);
        SetUrl(url);
    }

    public void Update(string publicId, string url)
    {
        SetPublicId(publicId);
        SetUrl(url);
        UpdateModificationDate();
    }

    private void SetPublicId(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            throw new ArgumentException("PublicId cannot be empty.", nameof(publicId));

        PublicId = publicId.Trim();
    }

    private void SetUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Url cannot be empty.", nameof(url));

        var sanitizedUrl = url.Trim();

        if (!Uri.TryCreate(sanitizedUrl, UriKind.Absolute, out _))
            throw new ArgumentException($"The string '{sanitizedUrl}' is not a valid absolute URL.", nameof(url));

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".avif" };

        var hasImageExtension =
            allowedExtensions.Any(ext => sanitizedUrl.EndsWith(ext, StringComparison.OrdinalIgnoreCase));

        if (!hasImageExtension)
        {
            throw new ArgumentException(
                $"The URL '{sanitizedUrl}' does not point to a supported image format. " +
                $"Allowed: {string.Join(", ", allowedExtensions)}", nameof(url));
        }

        Url = sanitizedUrl;
    }
}