namespace Watchwi.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateModificationDate()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}