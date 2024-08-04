using Microsoft.EntityFrameworkCore;
using Rent.Shared.Library.Entities;

namespace Rent.Renter.Infra.Data.Contexts;

public sealed class RenterContext : DbContext
{
    public RenterContext(DbContextOptions<RenterContext> options):base(options)
    {
        Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RenterContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.Entries().ToList().ForEach(e =>
        {
            switch (e)
            {
                case { State: EntityState.Added, Entity: Entity addedEntity }:
                    addedEntity.CreatedAtUtc = DateTime.UtcNow;
                    addedEntity.UpdatedAtUtc = DateTime.UtcNow;
                    break;
                case { State: EntityState.Modified, Entity: Entity updatedEntity }:
                    updatedEntity.UpdatedAtUtc = DateTime.UtcNow;
                    break;
                case { State: EntityState.Deleted, Entity: Entity deletedEntity }:
                    e.State = EntityState.Modified;
                    deletedEntity.DeletedAtUtc = DateTime.UtcNow;
                    deletedEntity.IsDeleted = true;
                    break;
            }
        });

        return base.SaveChangesAsync(cancellationToken);
    }
}