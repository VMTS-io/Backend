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
            .WithMany(u => u.MechanicMaintenaceFinalReports)
            .HasForeignKey(mir => mir.MechanicId)
            .OnDelete(DeleteBehavior.Cascade);

        // builder
        //     .HasOne(mfr => mfr.MaintenanceCategory)
        //     .WithMany()
        //     .HasForeignKey(mfr => mfr.MaintenanceCategoryId);

        builder
            .HasOne(mfr => mfr.Vehicle)
            .WithMany(v => v.MaintenaceFinalReports)
            .HasForeignKey(mir => mir.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(mfr => mfr.InitialReport)
            .WithOne()
            .HasForeignKey<MaintenanceFinalReport>(mir => mir.InitialReportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(mfr => mfr.MaintenaceRequest)
            .WithOne(mr => mr.FinalReport)
            .HasForeignKey<MaintenanceFinalReport>(mir => mir.MaintenaceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(mfr => mfr.TotalCost).HasColumnType("decimal(18,2)");
    }
}
