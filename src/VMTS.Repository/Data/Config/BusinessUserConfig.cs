using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.User_Business;

namespace VMTS.Repository.Data.Config;

public class BusinessUserConfig : IEntityTypeConfiguration<BusinessUser>
{
    public void Configure(EntityTypeBuilder<BusinessUser> builder)
    {
        #region TripRequest
        builder
            .HasMany(m => m.ManagerTripRequest)
            .WithOne(t => t.Manager)
            .HasForeignKey(t => t.ManagerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(m => m.DriverTripRequest)
            .WithOne(t => t.Driver)
            .HasForeignKey(t => t.DriverId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Trip Reports
        builder
            .HasMany(m => m.DriverTripReport)
            .WithOne(t => t.Driver)
            .HasForeignKey("DriverId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region FaultReport
        builder
            .HasMany(m => m.DriverFaultReport)
            .WithOne(t => t.Driver)
            .HasForeignKey("DriverId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region  MaintenaceReports
        // builder
        //     .HasMany(b => b.MechanicMaintenaceInitialReports)
        //     .WithOne(mir => mir.Mechanic)
        //     .HasForeignKey(mir => mir.MechanicId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Restrict);
        // builder
        //     .HasMany(b => b.MechanicMaintenaceFinalReports)
        //     .WithOne(m => m.Mechanic)
        //     .HasForeignKey(mir => mir.MechanicId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region MaintenanceRequest
        builder
            .HasMany(b => b.ManagerMaintenaceRequests)
            .WithOne(m => m.Manager)
            .HasForeignKey(mr => mr.ManagerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(b => b.MechanicMaintenaceRequests)
            .WithOne(m => m.Mechanic)
            .HasForeignKey(m => m.MechanicId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        #endregion
    }
}
