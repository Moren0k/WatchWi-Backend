using Watchwi.Domain.Common;
using Watchwi.Domain.Entities.Medias;

namespace Watchwi.Domain.Entities.Categories;

public class Category : BaseEntity
{
    public string Name { get; private set; } = null!;
    public ICollection<Media> Medias { get; private set; } = new List<Media>();

    public Category(string name)
    {
        SetName(name);
    }

    private void SetName(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null.");
        }

        var sanitizedName = name.Trim();

        if (string.IsNullOrWhiteSpace(sanitizedName))
        {
            throw new ArgumentException("Name cannot be empty or consist only of white spaces.", nameof(name));
        }

        if (sanitizedName.Length < 3)
        {
            throw new ArgumentException("Name is too short. It must be at least 3 characters.", nameof(name));
        }

        Name = sanitizedName;
    }
}