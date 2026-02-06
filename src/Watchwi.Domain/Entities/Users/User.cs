using Watchwi.Domain.Common;
using Watchwi.Domain.Entities.Images;
using Watchwi.Domain.Entities.Medias;

namespace Watchwi.Domain.Entities.Users;

public class User : BaseEntity
{
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; } = UserRole.User;

    public Guid? ProfileImageId { get; private set; }
    public Image? ProfileImage { get; private set; }

    public bool Status { get; private set; }
    public UserPlan Plan { get; private set; } = UserPlan.None;
    
    public ICollection<Media> FavoriteMedias { get; private set; } = new List<Media>();

    protected User()
    {
        // Required by EF Core
    }

    public User(string username, string email, string passwordHash)
    {
        SetUsername(username);
        SetEmail(email);
        SetPasswordHash(passwordHash);
    }

    public void UpdateProfile(string username)
    {
        SetUsername(username);
    }

    private void SetUsername(string username)
    {
        if (username == null)
        {
            throw new ArgumentNullException(nameof(username), "Name cannot be null.");
        }

        var sanitizedName = username.Trim();

        if (string.IsNullOrWhiteSpace(sanitizedName))
        {
            throw new ArgumentException("Name cannot be empty or consist only of white spaces.", nameof(username));
        }

        if (sanitizedName.Length < 3)
        {
            throw new ArgumentException("Name is too short. It must be at least 3 characters.", nameof(username));
        }

        Username = sanitizedName;
    }

    private void SetEmail(string email)
    {
        if (email == null)
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null.");
        }

        var sanitizedEmail = email.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(sanitizedEmail))
        {
            throw new ArgumentException("Email cannot be empty or consist only of white spaces.", nameof(email));
        }

        if (!sanitizedEmail.Contains('@') || !sanitizedEmail.Contains('.') || sanitizedEmail.Length < 5)
        {
            throw new ArgumentException($"The email format '{sanitizedEmail}' is invalid.", nameof(email));
        }

        Email = sanitizedEmail;
    }

    private void SetPasswordHash(string passwordHash)
    {
        if (passwordHash == null)
        {
            throw new ArgumentNullException(nameof(passwordHash));
        }

        var sanitizedHash = passwordHash.Trim();

        if (string.IsNullOrWhiteSpace(sanitizedHash))
        {
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
        }

        if (sanitizedHash.Length < 40)
        {
            throw new ArgumentException(
                "The provided string does not look like a valid password hash. It is too short.", nameof(passwordHash));
        }

        if (sanitizedHash.Contains(' '))
        {
            throw new ArgumentException("Password hash cannot contain internal spaces.", nameof(passwordHash));
        }

        PasswordHash = sanitizedHash;
    }

    public void SetRole(UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role) || role == UserRole.None)
        {
            throw new ArgumentException("The provided role is invalid.", nameof(role));
        }

        if (Role == role)
            return;

        Role = role;
    }

    public void SetProfileImage(Image image)
    {
        ProfileImage = image ?? throw new ArgumentNullException(nameof(image));
        ProfileImageId = image.Id;
    }

    public void SetStatus(bool status)
    {
        if (Status == status) return;

        Status = status;
    }

    public void SetPlan(UserPlan plan)
    {
        if (!Enum.IsDefined(typeof(UserPlan), plan) || plan == UserPlan.None)
        {
            throw new ArgumentException("The provided plan is invalid.", nameof(plan));
        }

        if (Plan == plan)
            return;

        Plan = plan;
    }
    
    public void MarkAsFavorite(Media media)
    {
        if (FavoriteMedias.All(m => m.Id != media.Id))
        {
            FavoriteMedias.Add(media);
        }
    }
    
    public void RemoveAsFavorite(Media media)
    {
        var existing = FavoriteMedias.FirstOrDefault(m => m.Id == media.Id);
        if (existing != null)
        {
            FavoriteMedias.Remove(existing);
        }
    }
}