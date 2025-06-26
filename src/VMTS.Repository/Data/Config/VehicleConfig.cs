using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Repository.Data.Config;

public class VehicleConfig : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder
            .HasOne(v => v.VehicleModel)
            .WithMany(m => m.Vehicle)
            .HasForeignKey("ModelId")
            .IsRequired();

        builder
            .HasMany(v => v.TripRequests)
            .WithOne(t => t.Vehicle)
            .HasForeignKey("VehicleId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(v => v.TripReports)
            .WithOne(t => t.Vehicle)
            .HasForeignKey("VehicleId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(v => v.FaultReports)
            .WithOne(t => t.Vehicle)
            .HasForeignKey(fr => fr.VehicleId)
            .IsRequired();

        builder
            .HasMany(v => v.MaintenaceRequests)
            .WithOne(r => r.Vehicle)
            .HasForeignKey(M => M.VehicleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(v => v.MaintenanceTrackings)
            .WithOne(m => m.Vehicle)
            .HasForeignKey(m => m.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(v => v.MaintenanceFinalReportParts)
            .WithOne(m => m.Vehicle)
            .HasForeignKey(m => m.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
