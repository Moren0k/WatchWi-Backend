namespace Watchwi.Application.IProviders.Cloudinary;

public interface ICloudinaryProvider
{
    Task<UploadMediaResponse> UploadAsync(UploadMediaRequest request);
    Task DeleteAsync(string publicId);
}