namespace Rent.Shared.Library.Entities;

public class Entity : BaseEntity
{
    public Guid Id { get; set; }
    
    protected Entity(Guid id, DateTime createdAtUtc, DateTime updatedAtUtc, DateTime? deletedAtUtc, bool isDeleted) : this(createdAtUtc, updatedAtUtc, deletedAtUtc, isDeleted)
    {
        Id = id;
    }
    
    protected Entity(DateTime createdAtUtc, DateTime updatedAtUtc, DateTime? deletedAtUtc, bool isDeleted) : base(createdAtUtc, updatedAtUtc, deletedAtUtc, isDeleted)
    {

    }

    protected Entity() : this(DateTime.UtcNow, DateTime.UtcNow, null, false)
    {
    }
    
    protected Entity(Guid id) : this(DateTime.UtcNow, DateTime.UtcNow, null, false)
    {
        Id = id;
    }
}