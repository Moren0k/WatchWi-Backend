using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Watchwi.Application.IProviders.Cloudinary;

namespace Watchwi.Infrastructure.Providers.Cloudinary;

public class CloudinaryProvider : ICloudinaryProvider
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryProvider(IOptions<CloudinarySettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var settings = options.Value;

        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);

        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    public async Task<UploadMediaResponse> UploadAsync(UploadMediaRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        if (request.FileStream == Stream.Null)
            throw new ArgumentException("FileStream is required");
        
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.FileName, request.FileStream),
            Folder = request.Folder,
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");

        return new UploadMediaResponse
        {
            PublicId = result.PublicId,
            Url = result.SecureUrl.AbsoluteUri
        };
    }

    public async Task DeleteAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Result != "ok")
        {
            throw new InvalidOperationException(
                $"Cloudinary delete failed for publicId '{publicId}'"
            );
        }
    }
}