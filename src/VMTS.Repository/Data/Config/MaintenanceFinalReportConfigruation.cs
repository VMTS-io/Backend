using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Data.Config;

public class MaintenanceFinalReportConfigruation : IEntityTypeConfiguration<MaintenanceFinalReport>
{
    public void Configure(EntityTypeBuilder<MaintenanceFinalReport> builder)
    {
        builder
            .HasOne(mfr => mfr.Mechanic)
            .WithMany()
            .HasForeignKey(mir => mir.MechanicId)
            .OnDelete(DeleteBehavior.Cascade);

        // builder
        //     .HasOne(mfr => mfr.Manager)
        //     .WithMany()
        //     .HasForeignKey(mir => mir.Manager)
        //     .OnDelete(DeleteBehavior.Cascade);


        builder
            .HasOne(mfr => mfr.MaintenanceCategory)
            .WithMany()
            .HasForeignKey(mfr => mfr.MaintenanceCategoryId);

        builder
            .HasOne(mfr => mfr.Vehicle)
            .WithMany()
            .HasForeignKey(mir => mir.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(mfr => mfr.InitialReport)
            .WithMany()
            .HasForeignKey(mir => mir.InitialReportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(mfr => mfr.MaintenaceRequest)
            .WithMany()
            .HasForeignKey(mir => mir.MaintenaceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mfr => mfr.MaintenanceCategory).WithMany();
        // builder.HasMany(mfr => mfr.ChangedPartss).WithMany();
        builder.Property(mfr => mfr.TotalCost).HasColumnType("decimal(18,2)");
    }
}
