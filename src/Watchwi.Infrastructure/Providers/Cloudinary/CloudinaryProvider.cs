using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Watchwi.Application.IProviders.Cloudinary;
using Watchwi.Application.Models.Cloudinary;

namespace Watchwi.Infrastructure.Providers.Cloudinary;

public class CloudinaryProvider : ICloudinaryProvider
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryProvider(IOptions<CloudinarySettings> options)
    {
        var settings = options.Value;

        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);

        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    public async Task<UploadMediaResponse> UploadAsync(UploadMediaRequest request)
    {
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
            throw new InvalidOperationException(result.Error.Message);

        return new UploadMediaResponse
        {
            PublicId = result.PublicId,
            Url = result.SecureUrl.AbsoluteUri
        };
    }

    public async Task DeleteAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}