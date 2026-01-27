using Watchwi.Domain.Common.IRepositories;

namespace Watchwi.Domain.Entities.Images;

public interface IImageRepository : IBaseRepository<Image>
{
    Task<Image?> GetByPublicIdAsync(string publicId);
}