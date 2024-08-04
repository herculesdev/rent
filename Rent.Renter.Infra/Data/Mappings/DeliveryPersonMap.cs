using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rent.Renter.Core.Entities;
using Rent.Shared.Library.Extensions;

namespace Rent.Renter.Infra.Data.Mappings;

public class DeliveryPersonMap : IEntityTypeConfiguration<DeliveryPerson>
{
    public void Configure(EntityTypeBuilder<DeliveryPerson> builder)
    {
        builder.ToTable("DeliveryPerson")
            .HasKey(x => x.Id);

        builder.HasIndex(x => x.DocumentNumber).IsUnique();
        builder.HasIndex(x => x.DriverLicenseNumber).IsUnique();
        
        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.DocumentNumber).HasColumnName("DocumentNumber");
        builder.Property(x => x.DriverLicenseNumber).HasColumnName("DriverLicenseNumber");
        builder.Property(x => x.DriverLicenseType).HasColumnName("DriverLicenseType");
        builder.Property(x => x.DriverLicenseImageName).HasColumnName("DriverLicenseImageName");
        builder.Property(x => x.BirthDate).HasColumnName("BirthDate");
        
        builder.MapAuditFields();
    }
}