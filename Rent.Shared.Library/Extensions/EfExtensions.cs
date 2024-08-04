using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rent.Shared.Library.Entities;

namespace Rent.Shared.Library.Extensions;

public static class EfExtensions
{
    public static void MapAuditFields<TEntity>(this EntityTypeBuilder<TEntity> b) where TEntity : Entity
    {
        b.Property<bool>("IsDeleted").HasColumnName("IsDeleted");
        b.Property<DateTime?>("DeletedAtUtc").HasColumnName("DeletedAtUtc");
        b.Property<DateTime>("CreatedAtUtc").HasColumnName("CreatedAtUtc");
        b.Property<DateTime>("UpdatedAtUtc").HasColumnName("UpdatedAtUtc");
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}