using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rent.Renter.Core.Entities;
using Rent.Shared.Library.Extensions;

namespace Rent.Renter.Infra.Data.Mappings;

public class RentalMap : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("Rental")
            .HasKey(x => x.Id);
        
        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.DeliveryPersonId).HasColumnName("DeliveryPersonId");
        builder.Property(x => x.PlanId).HasColumnName("PlanId");
        builder.Property(x => x.MotorbikeId).HasColumnName("MotorbikeId");
        builder.Property(x => x.StartDateUtc).HasColumnName("StartDateUtc");
        builder.Property(x => x.EstimatedEndDateUtc).HasColumnName("EstimatedEndDateUtc");
        builder.Property(x => x.EffectiveEndDateUtc).HasColumnName("EffectiveEndDateUtc");

        builder.HasOne(x => x.DeliveryPerson)
            .WithMany()
            .HasForeignKey(x => x.DeliveryPersonId);


        builder.Ignore(x => x.Plan);
        
        builder.MapAuditFields();
    }
}