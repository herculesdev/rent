using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rent.Backoffice.Core.Entities;
using Rent.Shared.Library.Extensions;

namespace Rent.Backoffice.Infra.Data.Mappings;

public class MotorbikeMap : IEntityTypeConfiguration<Motorbike>
{
    public void Configure(EntityTypeBuilder<Motorbike> builder)
    {
        builder.ToTable("Motorbike")
            .HasKey(x => x.Id);

        builder.HasIndex(x => x.LicensePlate).IsUnique();
        
        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.ManufactureYear).HasColumnName("ManufactureYear");
        builder.Property(x => x.ModelName).HasColumnName("ModelName");
        builder.Property(x => x.LicensePlate);
        
        builder.MapAuditFields();
    }
}