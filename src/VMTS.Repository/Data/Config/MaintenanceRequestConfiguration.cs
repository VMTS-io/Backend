using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Identity.Configurations;

public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenaceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenaceRequest> builder)
    {
        builder
            .HasOne(MR => MR.Vehicle)
            .WithMany(V => V.MaintenaceRequests)
            .HasForeignKey(MR => MR.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(m => m.MaintenanceCategory)
            .WithMany()
            .HasForeignKey(m => m.MaintenanceCategoryId)
            .IsRequired(false);
        builder
            .HasOne(m => m.InitialReport)
            .WithOne(mfr => mfr.MaintenanceRequest)
            .HasForeignKey<MaintenanceInitialReport>(m => m.MaintenanceRequestId);
        builder
            .HasOne(m => m.FinalReport)
            .WithOne(mfr => mfr.MaintenaceRequest)
            .HasForeignKey<MaintenanceFinalReport>(m => m.MaintenaceRequestId);
    }
}
