using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Data.Config;

public class MaintenanceInitialReportConfiguration
    : IEntityTypeConfiguration<MaintenanceInitialReport>
{
    public void Configure(EntityTypeBuilder<MaintenanceInitialReport> builder)
    {
        builder.HasMany(mir => mir.MissingParts).WithMany();

        builder.HasMany(mir => mir.MaintenanceCategories).WithMany();

        builder
            .HasOne(mir => mir.Mechanic)
            .WithMany()
            .HasForeignKey(mir => mir.MechanicId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(mir => mir.Vehicle)
            .WithMany()
            .HasForeignKey(mir => mir.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(mir => mir.MaintenaceRequest)
            .WithOne()
            .HasForeignKey<MaintenanceInitialReport>(mir => mir.MaintenaceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(mir => mir.ExpectedCost).HasColumnType("decimal(18,2)");
    }
}
