namespace Watchwi.Application.Models.Cloudinary;

public class UploadMediaRequest
{
    public Stream FileStream { get; init; } = null!;
    public string FileName { get; init; } = null!;
    public string? Folder { get; init; }
}