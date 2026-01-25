using Watchwi.Domain.Common;
using Watchwi.Domain.Entities.Categories;
using Watchwi.Domain.Entities.Images;
using Watchwi.Domain.Entities.Users;

namespace Watchwi.Domain.Entities.Medias;

public class Media : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public MediaType MediaType { get; private set; } = MediaType.None;
    
    public Guid? PosterId { get; private set; }
    public Image? Poster { get; private set; }
    
    public ICollection<User> FavoritedBy { get; private set; } = new List<User>();
    public ICollection<Category> Categories { get; private set; } = new List<Category>();

    protected Media()
    {
        // Requerido por EF Core
    }

    public Media(string title, string description, MediaType mediaType, Image poster)
    {
        SetTitle(title);
        SetDescription(description);
        SetMediaType(mediaType);
        SetPoster(poster);
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        var sanitizedTitle = title.Trim();

        if (sanitizedTitle.Length is < 2 or > 200)
            throw new ArgumentException("Title must be between 2 and 200 characters.", nameof(title));

        Title = sanitizedTitle;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));

        var sanitizedDescription = description.Trim();

        if (sanitizedDescription.Length < 10)
            throw new ArgumentException("Description is too short (min. 10 characters).", nameof(description));

        Description = sanitizedDescription;
    }

    private void SetMediaType(MediaType mediaType)
    {
        if (!Enum.IsDefined(typeof(MediaType), mediaType) || mediaType == MediaType.None)
            throw new ArgumentException("A valid media type must be provided.", nameof(mediaType));

        MediaType = mediaType;
    }

    private void SetPoster(Image image)
    {
        Poster = image ?? throw new ArgumentNullException(nameof(image));
        PosterId = image.Id;
    }
    
    public void AddCategory(Category? category)
    {
        if (category == null) return;
        if (Categories.All(c => c.Id != category.Id))
        {
            Categories.Add(category);
        }
    }
}