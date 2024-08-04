namespace Rent.Shared.Library.Entities;

public class BaseEntity
{
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
    
    protected BaseEntity(DateTime createdAtUtc, DateTime updatedAtUtc, DateTime? deletedAtUtc, bool isDeleted)
    {
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
        DeletedAtUtc = deletedAtUtc;
        IsDeleted = isDeleted;
    }

    protected BaseEntity() : this(DateTime.UtcNow, DateTime.UtcNow, null, false)
    {
    }
}